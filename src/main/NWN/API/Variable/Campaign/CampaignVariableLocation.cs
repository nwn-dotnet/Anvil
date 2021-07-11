using NWN.Core;

namespace NWN.API
{
  public sealed class CampaignVariableLocation : CampaignVariable<Location>
  {
    public override Location Value
    {
      get => NWScript.GetCampaignLocation(Campaign, Name, Player?.ControlledCreature);
      set => NWScript.SetCampaignLocation(Campaign, Name, value, Player?.ControlledCreature);
    }
  }
}
