using NWN;

namespace NWM.API
{
  public static class IntegerExtensions
  {
    public static bool ToBool(this int value)
    {
      return value == NWScript.TRUE;
    }

    public static int ToInt(this bool value)
    {
      return value ? NWScript.TRUE : NWScript.FALSE;
    }
  }
}