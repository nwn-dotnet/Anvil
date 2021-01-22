using NWN.Native.API;

namespace NWN.API
{
  public static class ExoLocStringExtensions
  {
    public static string ExtractLocString(this CExoLocString locStr, int nID = 0, byte gender = 0)
    {
      CExoString str = new CExoString();
      locStr.GetStringLoc(nID, str, gender);

      return str.ToString();
    }
  }
}
