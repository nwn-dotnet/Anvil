using System;
using Anvil.Internal;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  /// <summary>
  /// Service for redirecting native NWN server logs to Anvil (NLog)
  /// </summary>
  [ServiceBinding(typeof(ServerLogRedirectorService))]
  internal sealed unsafe class ServerLogRedirectorService : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly bool callOriginal;
    private readonly FunctionHook<ExecuteCommandPrintStringHook>? executeCommandPrintStringHook;
    private readonly FunctionHook<WriteToErrorFileHook>? writeToErrorFileHook;

    private readonly FunctionHook<WriteToLogFileHook>? writeToLogFileHook;
    private bool printString;

    public ServerLogRedirectorService(HookService hookService)
    {
      switch (EnvironmentConfig.LogMode)
      {
        case LogMode.Default:
          return;
        case LogMode.Duplicate:
          callOriginal = true;
          break;
        case LogMode.Redirect:
          callOriginal = false;
          break;
      }

      writeToLogFileHook = hookService.RequestHook<WriteToLogFileHook>(OnWriteToLogFile, FunctionsLinux._ZN17CExoDebugInternal14WriteToLogFileERK10CExoString, HookOrder.VeryEarly);
      writeToErrorFileHook = hookService.RequestHook<WriteToErrorFileHook>(OnWriteToErrorFile, FunctionsLinux._ZN17CExoDebugInternal16WriteToErrorFileERK10CExoString, HookOrder.VeryEarly);
      executeCommandPrintStringHook = hookService.RequestHook<ExecuteCommandPrintStringHook>(OnExecuteCommandPrintString, FunctionsLinux._ZN25CNWVirtualMachineCommands25ExecuteCommandPrintStringEii, HookOrder.VeryEarly);
    }

    private delegate int ExecuteCommandPrintStringHook(void* pVirtualMachineCommands, int nCommandId, int nParameters);

    private delegate void WriteToErrorFileHook(void* pExoDebugInternal, void* pMessage);

    private delegate void WriteToLogFileHook(void* pExoDebugInternal, void* pMessage);

    public void Dispose()
    {
      writeToLogFileHook?.Dispose();
      writeToErrorFileHook?.Dispose();
      executeCommandPrintStringHook?.Dispose();
    }

    private int OnExecuteCommandPrintString(void* pVirtualMachineCommands, int nCommandId, int nParameters)
    {
      printString = true;
      int retVal = executeCommandPrintStringHook!.CallOriginal(pVirtualMachineCommands, nCommandId, nParameters);
      printString = false;
      return retVal;
    }

    private void OnWriteToErrorFile(void* pExoDebugInternal, void* pMessage)
    {
      CExoString message = CExoString.FromPointer(pMessage);
      Log.Error(TrimMessage(message));

      if (callOriginal)
      {
        writeToErrorFileHook!.CallOriginal(pExoDebugInternal, pMessage);
      }
    }

    private void OnWriteToLogFile(void* pExoDebugInternal, void* pMessage)
    {
      CExoString message = CExoString.FromPointer(pMessage);
      Log.Info(TrimMessage(message));

      if (callOriginal)
      {
        writeToLogFileHook!.CallOriginal(pExoDebugInternal, pMessage);
      }
    }

    private string TrimMessage(CExoString message)
    {
      string retVal = message.ToString();

      if (!printString)
      {
        int startIndex = retVal.IndexOf(']') + 1;
        retVal = retVal[startIndex..];
      }

      return retVal.Trim();
    }
  }
}
