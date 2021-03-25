using System;

namespace NWN.API
{
  public static class NativeScript
  {
    [Obsolete("Moved to VirtualMachine.Execute")]
    public static void Execute(string scriptName, NwObject target, params (string ParamName, string ParamValue)[] scriptParams)
      => VirtualMachine.Execute(scriptName, target, scriptParams);

    [Obsolete("Moved to VirtualMachine.Execute")]
    public static void Execute(string scriptName, params (string ParamName, string ParamValue)[] scriptParams)
      => VirtualMachine.Execute(scriptName, null, scriptParams);
  }
}
