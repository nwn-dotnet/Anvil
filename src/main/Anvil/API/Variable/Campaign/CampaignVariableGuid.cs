using System;
using NWN.Core;

namespace Anvil.API
{
  public sealed class CampaignVariableGuid : CampaignVariable<Guid>
  {
    public override Guid Value
    {
      get
      {
        string stored = NWScript.GetCampaignString(Campaign, Name, Player?.ControlledCreature);
        return string.IsNullOrEmpty(stored) ? Guid.Empty : Guid.Parse(stored);
      }
      set => NWScript.SetCampaignString(Campaign, Name, value.ToUUIDString(), Player?.ControlledCreature);
    }
  }
}
