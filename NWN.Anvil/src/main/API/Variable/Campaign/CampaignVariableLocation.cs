using System;
using NWN.Core;

namespace Anvil.API
{
  public sealed class CampaignVariableLocation : CampaignVariable<Location?>
  {
    public override Location? Value
    {
      get => NWScript.GetCampaignLocation(Campaign, Name, Player?.ControlledCreature);
      set => NWScript.SetCampaignLocation(Campaign, Name, value ?? throw new ArgumentNullException(nameof(value)), Player?.ControlledCreature);
    }
  }
}
