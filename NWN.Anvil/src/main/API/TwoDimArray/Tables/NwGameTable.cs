using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  public class NwGameTable
  {
    public static TwoDimArray<IPRPCostTableEntry>[] IPRPCostTables { get; private set; }
    public static TwoDimArray<AppearanceTableEntry> AppearanceTable { get; private set; }

    [ServiceBinding(typeof(Factory))]
    [ServiceBindingOptions(InternalBindingPriority.API)]
    internal sealed unsafe class Factory
    {
      public Factory()
      {
        LoadTables();
      }

      internal void LoadTables()
      {
        CTwoDimArrays twoDimArrays = NWNXLib.Rules().m_p2DArrays;
        LoadCostTables(twoDimArrays);
        AppearanceTable = new TwoDimArray<AppearanceTableEntry>(twoDimArrays.m_pAppearanceTable);
      }

      private void LoadCostTables(CTwoDimArrays twoDimArrays)
      {
        IPRPCostTables = new TwoDimArray<IPRPCostTableEntry>[twoDimArrays.m_nNumIPRPCostTables];
        void** costTableArray = twoDimArrays.m_paIPRPCostTables;

        for (int i = 0; i < IPRPCostTables.Length; i++)
        {
          IPRPCostTables[i] = new TwoDimArray<IPRPCostTableEntry>(C2DA.FromPointer(costTableArray[i]));
        }
      }
    }
  }
}
