using NWN.Core;

namespace Anvil.API
{
  public sealed class CampaignVariableString : CampaignVariable<string>
  {
    public override string Value
    {
      get => NWScript.GetCampaignString(Campaign, Name, Player?.ControlledCreature);
      set => NWScript.SetCampaignString(Campaign, Name, value, Player?.ControlledCreature);
    }
  }
}
