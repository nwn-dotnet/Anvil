using System;
using Anvil.Internal;
using Anvil.Native;
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
    private readonly FunctionHook<Functions.CVirtualMachineCmdImplementer.ExecuteCommandPrintString>? executeCommandPrintStringHook;
    private readonly FunctionHook<Functions.CExoDebugInternal.WriteToErrorFile>? writeToErrorFileHook;

    private readonly FunctionHook<Functions.CExoDebugInternal.WriteToLogFile>? writeToLogFileHook;
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

      writeToLogFileHook = hookService.RequestHook<Functions.CExoDebugInternal.WriteToLogFile>(OnWriteToLogFile, HookOrder.VeryEarly);
      writeToErrorFileHook = hookService.RequestHook<Functions.CExoDebugInternal.WriteToErrorFile>(OnWriteToErrorFile, HookOrder.VeryEarly);
      executeCommandPrintStringHook = hookService.RequestHook<Functions.CVirtualMachineCmdImplementer.ExecuteCommandPrintString>(OnExecuteCommandPrintString, HookOrder.VeryEarly);
    }

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
      string? retVal = message.ToString();
      if (string.IsNullOrEmpty(retVal))
      {
        return string.Empty;
      }

      if (!printString)
      {
        int startIndex = retVal.IndexOf(']') + 1;
        retVal = retVal[startIndex..];
      }

      return retVal.Trim();
    }
  }
}
