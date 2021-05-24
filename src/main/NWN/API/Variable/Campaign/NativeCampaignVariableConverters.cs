using System.Numerics;
using NWN.Core;

namespace NWN.API
{
  [CampaignVariableConverter(typeof(float), typeof(int), typeof(Location), typeof(string), typeof(Vector3))]
  internal class NativeCampaignVariableConverters : ICampaignVariableConverter<float>, ICampaignVariableConverter<int>, ICampaignVariableConverter<Location>, ICampaignVariableConverter<string>, ICampaignVariableConverter<Vector3>
  {
    float ICampaignVariableConverter<float>.GetCampaign(string campaign, string name, NwPlayer player)
      => NWScript.GetCampaignFloat(campaign, name, player.ControlledCreature);

    void ICampaignVariableConverter<float>.SetCampaign(string campaign, string name, float value, NwPlayer player)
      => NWScript.SetCampaignFloat(campaign, name, value, player.ControlledCreature);

    void ICampaignVariableConverter<float>.ClearCampaign(string campaign, string name, NwPlayer player)
      => NWScript.DeleteCampaignVariable(campaign, name, player.ControlledCreature);

    int ICampaignVariableConverter<int>.GetCampaign(string campaign, string name, NwPlayer player)
      => NWScript.GetCampaignInt(campaign, name, player.ControlledCreature);

    void ICampaignVariableConverter<int>.SetCampaign(string campaign, string name, int value, NwPlayer player)
      => NWScript.SetCampaignInt(campaign, name, value, player.ControlledCreature);

    void ICampaignVariableConverter<int>.ClearCampaign(string campaign, string name, NwPlayer player)
      => NWScript.DeleteCampaignVariable(campaign, name, player.ControlledCreature);

    Location ICampaignVariableConverter<Location>.GetCampaign(string campaign, string name, NwPlayer player)
      => NWScript.GetCampaignLocation(campaign, name, player.ControlledCreature);

    void ICampaignVariableConverter<Location>.SetCampaign(string campaign, string name, Location value, NwPlayer player)
      => NWScript.SetCampaignLocation(campaign, name, value, player.ControlledCreature);

    void ICampaignVariableConverter<Location>.ClearCampaign(string campaign, string name, NwPlayer player)
      => NWScript.DeleteCampaignVariable(campaign, name, player.ControlledCreature);

    string ICampaignVariableConverter<string>.GetCampaign(string campaign, string name, NwPlayer player)
      => NWScript.GetCampaignString(campaign, name, player.ControlledCreature);

    void ICampaignVariableConverter<string>.SetCampaign(string campaign, string name, string value, NwPlayer player)
      => NWScript.SetCampaignString(campaign, name, value, player.ControlledCreature);

    void ICampaignVariableConverter<string>.ClearCampaign(string campaign, string name, NwPlayer player)
      => NWScript.DeleteCampaignVariable(campaign, name, player.ControlledCreature);

    Vector3 ICampaignVariableConverter<Vector3>.GetCampaign(string campaign, string name, NwPlayer player)
      => NWScript.GetCampaignVector(campaign, name, player.ControlledCreature);

    void ICampaignVariableConverter<Vector3>.SetCampaign(string campaign, string name, Vector3 value, NwPlayer player)
      => NWScript.SetCampaignVector(campaign, name, value, player.ControlledCreature);

    void ICampaignVariableConverter<Vector3>.ClearCampaign(string campaign, string name, NwPlayer player)
      => NWScript.DeleteCampaignVariable(campaign, name, player.ControlledCreature);
  }
}
