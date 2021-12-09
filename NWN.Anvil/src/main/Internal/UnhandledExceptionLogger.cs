using System;
using NLog;

namespace Anvil.Internal
{
  internal sealed class UnhandledExceptionLogger : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    ~UnhandledExceptionLogger()
    {
      Unregister();
    }

    public void Dispose()
    {
      Unregister();
      GC.SuppressFinalize(this);
    }

    public void Init()
    {
      Log.Info("Registering Unhandled Exception Logger");
      AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }

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
