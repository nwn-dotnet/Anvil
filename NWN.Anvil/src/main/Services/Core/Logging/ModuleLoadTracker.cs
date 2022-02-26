using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  internal sealed unsafe class ModuleLoadTracker : ICoreService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly HookService hookService;

    private FunctionHook<LoadModuleInProgressHook> loadModuleInProgressHook;

    private delegate uint LoadModuleInProgressHook(void* pModule, int nAreasLoaded, int nAreasToLoad);

    public ModuleLoadTracker(HookService hookService)
    {
      this.hookService = hookService;
    }

    void ICoreService.Init()
    {
      loadModuleInProgressHook = hookService.RequestHook<LoadModuleInProgressHook>(OnModuleLoadProgressChange, FunctionsLinux._ZN10CNWSModule20LoadModuleInProgressEii, HookOrder.Earliest);
    }

    void ICoreService.Load() {}

    void ICoreService.Start() {}

    void ICoreService.Shutdown() {}

    void ICoreService.Unload() {}

    private uint OnModuleLoadProgressChange(void* pModule, int nAreasLoaded, int nAreasToLoad)
    {
      CNWSModule module = CNWSModule.FromPointer(pModule);

      int index = nAreasLoaded;
      CExoLinkedListNode node = module.m_lstModuleArea.m_pcExoLinkedListInternal.pHead;

      while (node != null && index != 0)
      {
        node = node.pNext;
        index--;
      }

      CResRef resRef = node != null ? CResRef.FromPointer(node.pObject) : null;
      if (resRef != null)
      {
        Log.Debug("Loading area {Area} ({AreaNum}/{AreaCount})", resRef.ToString(), nAreasLoaded + 1, nAreasToLoad);
      }

      uint retVal = loadModuleInProgressHook.CallOriginal(pModule, nAreasLoaded, nAreasToLoad);
      if (resRef != null)
      {
        switch (retVal)
        {
          case 0:
            Log.Debug("Loaded area {Area} ({AreaNum}/{AreaCount})", resRef.ToString(), nAreasLoaded + 1, nAreasToLoad);
            break;
          default:
            Log.Error("Failed to load area {Area}, error code {ErrorCode} ({AreaNum}/{AreaCount})", resRef.ToString(), retVal, nAreasLoaded + 1, nAreasToLoad);
            break;
        }
      }

      return retVal;
    }
  }
}
