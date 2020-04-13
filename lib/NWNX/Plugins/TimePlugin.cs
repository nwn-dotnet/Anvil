namespace NWNX
{
  [NWNXPlugin(NWNX_Time)]
  internal class TimePlugin
  {
    public const string NWNX_Time = "NWNX_Time";

    /// /< @private
    /// / @brief Returns the current date.
    /// / @return The date in the format (mm/dd/yyyy).
    public static string GetSystemDate()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Time, "GetSystemDate");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopString();
    }

    /// / @brief Returns current time.
    /// / @return The current time in the format (24:mm:ss).
    public static string GetSystemTime()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Time, "GetSystemTime");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopString();
    }

    /// / @return Returns the number of seconds since midnight on January 1, 1970.
    public static int GetTimeStamp()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Time, "GetTimeStamp");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief A high resolution timestamp
    /// / @return Returns the number of microseconds since midnight on January 1, 1970.
    public static HighResTimestamp GetHighResTimeStamp()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Time, "GetHighResTimeStamp");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      HighResTimestamp retVal;
      retVal.microseconds = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.seconds = NWN.Internal.NativeFunctions.StackPopInteger();
      return retVal;
    }

    /// / @}
  }

  public struct HighResTimestamp
  {
    public int seconds;
    public int microseconds;
  }
}
