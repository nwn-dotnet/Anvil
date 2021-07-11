using NWN.Core;

namespace Anvil.API
{
  public sealed class CampaignVariableBool : CampaignVariable<bool>
  {
    public override bool Value
    {
      get => NWScript.GetCampaignInt(Campaign, Name, Player?.ControlledCreature).ToBool();
      set => NWScript.SetCampaignInt(Campaign, Name, value.ToInt(), Player?.ControlledCreature);
    }
  }
}
