namespace NWNX
{
  [NWNXPlugin(NWNX_Ruby)]
  internal class RubyPlugin
  {
    public const string NWNX_Ruby = "NWNX_Ruby";

    /// /< @private
    public static string Evaluate(string sCode)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Ruby, "Evaluate");
      NWN.Internal.NativeFunctions.StackPushString(sCode);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopString();
    }

    /// / @brief Evaluates some ruby code.
    /// / @param sCode The code to evaluate.
    /// / @return The output of the call.
    /// / @}
  }
}
