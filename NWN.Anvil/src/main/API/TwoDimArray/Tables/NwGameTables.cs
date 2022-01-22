using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  public static class NwGameTables
  {
    public static TwoDimArray<AppearanceTableEntry> AppearanceTable { get; private set; }

    [ServiceBinding(typeof(Factory))]
    [ServiceBindingOptions(InternalBindingPriority.API)]
    internal sealed class Factory
    {
      public Factory()
      {
        LoadTables();
      }

      internal void LoadTables()
      {
        CTwoDimArrays twoDimArrays = NWNXLib.Rules().m_p2DArrays;
        AppearanceTable = new TwoDimArray<AppearanceTableEntry>(twoDimArrays.m_pAppearanceTable);
      }
    }
  }
}
