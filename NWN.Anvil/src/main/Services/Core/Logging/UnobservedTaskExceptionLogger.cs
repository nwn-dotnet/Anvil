using System;
using System.Threading.Tasks;
using NLog;

namespace Anvil.Services
{
  [ServiceBindingOptions(InternalBindingPriority.High)]
  internal class UnobservedTaskExceptionLogger : ICoreService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    ~UnobservedTaskExceptionLogger()
    {
      Unregister();
    }

    void ICoreService.Init()
    {
      TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    void ICoreService.Load() {}

    void ICoreService.Shutdown()
    {
      Unregister();
      GC.SuppressFinalize(this);
    }

    void ICoreService.Start() {}

    void ICoreService.Unload() {}

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
      Log.Error(e.Exception, "Task Exception");
      e.SetObserved();
    }

    private void Unregister()
    {
      TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
    }
  }
}
