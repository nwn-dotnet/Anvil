using System.Numerics;
using NWN.Core;

namespace NWN.API
{
  public sealed class CampaignVariableVector : CampaignVariable<Vector3>
  {
    public override Vector3 Value
    {
      get => NWScript.GetCampaignVector(Campaign, Name, Player?.ControlledCreature);
      set => NWScript.SetCampaignVector(Campaign, Name, value, Player?.ControlledCreature);
    }
  }
}
