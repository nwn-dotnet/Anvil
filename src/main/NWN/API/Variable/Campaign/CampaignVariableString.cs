using NWN.Core;

namespace NWN.API
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
