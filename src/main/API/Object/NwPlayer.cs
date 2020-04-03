using NWN;
using NWNX;

namespace NWM.API
{
  public sealed class NwPlayer : NwCreature
  {
    internal NwPlayer(uint objectId) : base(objectId) {}

    public bool IsDM => NWScript.GetIsDM(ObjectId).ToBool();
    public string PlayerName => NWScript.GetPCPlayerName(this);

    public void SendServerMessage(string message, Color color)
    {
      NWScript.SendMessageToPC(this, message.ColorString(color));
    }

    public void SendServerMessage(string message)
    {
      NWScript.SendMessageToPC(this, message);
    }

    public void ForceOpenInventory(NwPlaceable target)
    {
      PluginUtils.AssertPluginExists<PlayerPlugin>();
      PlayerPlugin.ForcePlaceableInventoryWindow(this, target);
    }

    public void SetPlaceableNameOverride(NwPlaceable placeable, string name)
    {
      PluginUtils.AssertPluginExists<PlayerPlugin>();
      PlayerPlugin.SetPlaceableNameOverride(this, placeable, name);
    }

    public void ExportCharacter()
    {
      NWScript.ExportSingleCharacter(this);
    }
  }
}