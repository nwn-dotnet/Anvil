using System;
using NLog;

namespace Anvil.Services
{
  [ServiceBindingOptions(InternalBindingPriority.High)]
  internal sealed class UnhandledExceptionLogger : ICoreService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    ~UnhandledExceptionLogger()
    {
      Unregister();
    }

    void ICoreService.Init()
    {
      Log.Info("Registering Unhandled Exception Logger");
      AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }

    void ICoreService.Load() {}

    void ICoreService.Shutdown()
    {
      Unregister();
      GC.SuppressFinalize(this);
    }

    void ICoreService.Unload() {}

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs eventData)
    {
      Exception e = (Exception)eventData.ExceptionObject;
      if (eventData.IsTerminating)
      {
        Log.Fatal(e, "Unhandled Exception.");
      }
      else
      {
        Log.Error(e, "Unhandled Exception.");
      }
    }

    private void Unregister()
    {
      AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
    }
  }
}
