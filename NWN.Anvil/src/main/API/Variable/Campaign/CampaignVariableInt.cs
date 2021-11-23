using NWN.Core;

namespace Anvil.API
{
  public sealed class CampaignVariableInt : CampaignVariable<int>
  {
    public override int Value
    {
      get => NWScript.GetCampaignInt(Campaign, Name, Player?.ControlledCreature);
      set => NWScript.SetCampaignInt(Campaign, Name, value, Player?.ControlledCreature);
    }
  }
}
