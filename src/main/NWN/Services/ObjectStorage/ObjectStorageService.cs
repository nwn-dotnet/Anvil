using System;
using System.Collections.Generic;
using NLog;
using NWN.API;
using NWN.Native.API;

namespace NWN.Services
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

    private readonly Dictionary<ICGameObject, ObjectStorage> objectStorage = new Dictionary<ICGameObject, ObjectStorage>();

    public ObjectStorageService(HookService hookService)
    {
      objectDestructorHook = hookService.RequestHook<ObjectDestructorHook>(OnObjectDestructor, FunctionsLinux._ZN10CNWSObjectD1Ev, HookOrder.VeryEarly);
      areaDestructorHook = hookService.RequestHook<AreaDestructorHook>(OnAreaDestructor, FunctionsLinux._ZN8CNWSAreaD1Ev, HookOrder.VeryEarly);
      eatTURDHook = hookService.RequestHook<EatTURDHook>(OnEatTURD, FunctionsLinux._ZN10CNWSPlayer7EatTURDEP14CNWSPlayerTURD, HookOrder.VeryEarly);
      dropTURDHook = hookService.RequestHook<DropTURDHook>(OnDropTURD, FunctionsLinux._ZN10CNWSPlayer8DropTURDEv, HookOrder.VeryEarly);

      saveToGffHook = hookService.RequestHook<SaveToGffHook>(OnSaveToGff, FunctionsLinux._ZN8CNWSUUID9SaveToGffEP7CResGFFP10CResStruct, HookOrder.VeryEarly);
      loadFromGffHook = hookService.RequestHook<LoadFromGffHook>(OnLoadFromGff, FunctionsLinux._ZN8CNWSUUID11LoadFromGffEP7CResGFFP10CResStruct, HookOrder.VeryEarly);
    }

    public ObjectStorage GetObjectStorage(NwObject gameObject)
    {
      return GetObjectStorage(gameObject.Object);
    }

    public ObjectStorage GetObjectStorage(ICGameObject gameObject)
    {
      if (!objectStorage.TryGetValue(gameObject, out ObjectStorage storage))
      {
        storage = new ObjectStorage();
        objectStorage[gameObject] = storage;
      }

      return storage;
    }

    public void DestroyObjectStorage(NwObject gameObject)
    {
      DestroyObjectStorage(gameObject.Object);
    }

    public void DestroyObjectStorage(ICGameObject gameObject)
    {
      objectStorage.Remove(gameObject);
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

      objectStorage[playerObj] = GetObjectStorage(turd).Clone();
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

      if (turdList != null && (pHead = turdList.pHead) != null)
      {
        CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);
        CNWSPlayerTURD turd = CNWSPlayerTURD.FromPointer(pHead);

        ICGameObject playerObj = player.m_oidNWSObject.ToNwObject().Object;
        objectStorage[turd] = GetObjectStorage(playerObj).Clone();
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
