using NWN.Core;

namespace NWN.API
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
