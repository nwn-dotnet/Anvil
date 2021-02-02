using NWN.Native.API;

namespace NWN.API
{
  public static class NativeUtils
  {
    public static CExoLocString ToExoLocString(this string str, int nId = 0, byte gender = 0)
    {
      CExoLocString locString = new CExoLocString();
      locString.AddString(nId, new CExoString(str), gender);
      return locString;
    }

    public static CExoLocString ToExoLocString(this CExoString str, int nId = 0, byte gender = 0)
    {
      CExoLocString locString = new CExoLocString();
      locString.AddString(nId, str, gender);
      return locString;
    }

    public static string ExtractLocString(this CExoLocString locStr, int nID = 0, byte gender = 0)
    {
      CExoString str = new CExoString();
      locStr.GetStringLoc(nID, str, gender);

      return str.ToString();
    }
  }
}
