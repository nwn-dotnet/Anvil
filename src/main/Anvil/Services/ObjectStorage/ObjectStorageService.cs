using System;
using System.Collections.Generic;
using Anvil.API;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(ObjectStorageService))]
  internal sealed unsafe class ObjectStorageService
  {
    private static readonly byte* GffFieldNamePtr = "NWNX_POS".GetNullTerminatedString();

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private delegate void ObjectDestructorHook(void* pObject);

    private delegate void AreaDestructorHook(void* pArea);

    private delegate void EatTURDHook(void* pPlayer, void* pTURD);

    private delegate void DropTURDHook(void* pPlayer);

    private delegate void SaveToGffHook(void* pUUID, void* pRes, void* pStruct);

    private delegate int LoadFromGffHook(void* pUUID, void* pRes, void* pStruct);

    private readonly FunctionHook<ObjectDestructorHook> objectDestructorHook;
    private readonly FunctionHook<AreaDestructorHook> areaDestructorHook;
    private readonly FunctionHook<EatTURDHook> eatTURDHook;
    private readonly FunctionHook<DropTURDHook> dropTURDHook;

    private readonly FunctionHook<SaveToGffHook> saveToGffHook;
    private readonly FunctionHook<LoadFromGffHook> loadFromGffHook;

    private readonly Dictionary<IntPtr, ObjectStorage> objectStorage = new Dictionary<IntPtr, ObjectStorage>();

    public ObjectStorageService(HookService hookService)
    {
      objectDestructorHook = hookService.RequestHook<ObjectDestructorHook>(OnObjectDestructor, FunctionsLinux._ZN10CNWSObjectD1Ev, HookOrder.VeryEarly);
      areaDestructorHook = hookService.RequestHook<AreaDestructorHook>(OnAreaDestructor, FunctionsLinux._ZN8CNWSAreaD1Ev, HookOrder.VeryEarly);
      eatTURDHook = hookService.RequestHook<EatTURDHook>(OnEatTURD, FunctionsLinux._ZN10CNWSPlayer7EatTURDEP14CNWSPlayerTURD, HookOrder.VeryEarly);
      dropTURDHook = hookService.RequestHook<DropTURDHook>(OnDropTURD, FunctionsLinux._ZN10CNWSPlayer8DropTURDEv, HookOrder.VeryEarly);

      // We want to prioritize our call first for serialization, so it gets called last in the CallOriginal call in NWNX.
      const int orderBeforeNWNX = HookOrder.VeryEarly - 1;
      const int orderAfterNWNX = HookOrder.VeryEarly + 1;

      saveToGffHook = hookService.RequestHook<SaveToGffHook>(OnSaveToGff, FunctionsLinux._ZN8CNWSUUID9SaveToGffEP7CResGFFP10CResStruct, orderBeforeNWNX);
      loadFromGffHook = hookService.RequestHook<LoadFromGffHook>(OnLoadFromGff, FunctionsLinux._ZN8CNWSUUID11LoadFromGffEP7CResGFFP10CResStruct, orderAfterNWNX);
    }

    public ObjectStorage GetObjectStorage(NwObject gameObject)
    {
      return GetObjectStorage(gameObject.Object);
    }

    public ObjectStorage GetObjectStorage(ICGameObject gameObject)
    {
      if (!objectStorage.TryGetValue(gameObject.Pointer, out ObjectStorage storage))
      {
        storage = new ObjectStorage();
        objectStorage[gameObject.Pointer] = storage;
      }

      return storage;
    }

    public bool TryGetObjectStorage(NwObject gameObject, out ObjectStorage storage)
    {
      return TryGetObjectStorage(gameObject.Object, out storage);
    }

    public bool TryGetObjectStorage(ICGameObject gameObject, out ObjectStorage storage)
    {
      return objectStorage.TryGetValue(gameObject.Pointer, out storage);
    }

    public void DestroyObjectStorage(NwObject gameObject)
    {
      DestroyObjectStorage(gameObject.Object);
    }

    public void DestroyObjectStorage(ICGameObject gameObject)
    {
      objectStorage.Remove(gameObject.Pointer);
    }

    private void OnObjectDestructor(void* pObject)
    {
      CNWSObject gameObject = CNWSObject.FromPointer(pObject);
      objectDestructorHook.CallOriginal(pObject);
      DestroyObjectStorage(gameObject);
    }

    private void OnAreaDestructor(void* pArea)
    {
      CNWSArea area = CNWSArea.FromPointer(pArea);
      areaDestructorHook.CallOriginal(pArea);
      DestroyObjectStorage(area);
    }

    private void OnEatTURD(void* pPlayer, void* pTURD)
    {
      CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);
      CNWSPlayerTURD turd = CNWSPlayerTURD.FromPointer(pTURD);

      ICGameObject playerObj = player.m_oidNWSObject.ToNwObject().Object;

      objectStorage[playerObj.Pointer] = GetObjectStorage(turd).Clone();
      eatTURDHook.CallOriginal(pPlayer, pTURD);
    }

    private void OnDropTURD(void* pPlayer)
    {
      dropTURDHook.CallOriginal(pPlayer);

      // Be very, very paranoid. Bad things happen when the TURD list doesn't exist
      // This can happen when you BootPC() the very first PC to connect to your sever
      //     https://github.com/nwnxee/unified/issues/319
      CExoLinkedListInternal turdList = NwModule.Instance.Module.m_lstTURDList.m_pcExoLinkedListInternal;
      CExoLinkedListNode pHead;

      if (turdList != null && (pHead = turdList.pHead) != null && pHead.pObject != null)
      {
        CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);
        CNWSPlayerTURD turd = CNWSPlayerTURD.FromPointer(pHead.pObject);

        ICGameObject playerObj = player.m_oidNWSObject.ToNwObject().Object;
        objectStorage[turd.Pointer] = GetObjectStorage(playerObj).Clone();
      }
    }

    private void OnSaveToGff(void* pUUID, void* pRes, void* pStruct)
    {
      CNWSUUID uuid = CNWSUUID.FromPointer(pUUID);
      CResGFF resGff = CResGFF.FromPointer(pRes);
      CResStruct resStruct = CResStruct.FromPointer(pStruct);

      string serialized = GetObjectStorage(uuid.m_parent).Serialize();
      resGff.WriteFieldCExoString(resStruct, serialized.ToExoString(), GffFieldNamePtr);

      saveToGffHook.CallOriginal(pUUID, pRes, pStruct);
    }

    private int OnLoadFromGff(void* pUUID, void* pRes, void* pStruct)
    {
      CNWSUUID uuid = CNWSUUID.FromPointer(pUUID);
      CResGFF resGff = CResGFF.FromPointer(pRes);
      CResStruct resStruct = CResStruct.FromPointer(pStruct);

      int bSuccess;
      CExoString str = resGff.ReadFieldCExoString(resStruct, GffFieldNamePtr, &bSuccess);
      if (bSuccess.ToBool())
      {
        try
        {
          GetObjectStorage(uuid.m_parent).Deserialize(str.ToString());
        }
        catch (Exception e)
        {
          Log.Error(e, "Failed to load object storage.");
        }
      }

      return loadFromGffHook.CallOriginal(pUUID, pRes, pStruct);
    }
  }
}
