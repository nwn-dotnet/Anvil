using NWN.Core;

namespace Anvil.API
{
  public sealed class CampaignVariableFloat : CampaignVariable<float>
  {
    public override float Value
    {
      get => NWScript.GetCampaignFloat(Campaign, Name, Player?.ControlledCreature);
      set => NWScript.SetCampaignFloat(Campaign, Name, value, Player?.ControlledCreature);
    }
  }
}
