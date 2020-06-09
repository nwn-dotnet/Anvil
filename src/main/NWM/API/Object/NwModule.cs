using System.Collections.Generic;
using NWM.API.Constants;
using NWMX.API.Constants;
using NWN;

namespace NWM.API
{
  [NativeObjectInfo(0, InternalObjectType.Module)]
  public sealed class NwModule : NwObject
  {
    internal NwModule(uint objectId) : base(objectId) {}

    public static readonly NwModule Instance = new NwModule(NWScript.GetModule());

    public NwPlayer PCSpeaker => NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();

    public Spell SpellId => (Spell) NWScript.GetSpellId();

    public NwWaypoint GetWaypointByTag(string tag) => NWScript.GetWaypointByTag(tag).ToNwObject<NwWaypoint>();

    public Location StartingLocation => NWScript.GetStartingLocation();

    public IEnumerable<NwArea> Areas
    {
      get
      {
        for (uint area = NWScript.GetFirstArea(); area != INVALID; area = NWScript.GetNextArea())
        {
          yield return area.ToNwObject<NwArea>();
        }
      }
    }

    public IEnumerable<NwGameObject> GetObjectsByTag(string tag)
    {
      int i;
      uint obj;
      for (i = 0, obj = NWScript.GetObjectByTag(tag, i); obj != INVALID; i++, obj = NWScript.GetObjectByTag(tag, i))
      {
        yield return obj.ToNwObject<NwGameObject>();
      }
    }

    public IEnumerable<NwPlayer> Players
    {
      get
      {
        for (uint player = NWScript.GetFirstPC(); player != INVALID; player = NWScript.GetNextPC())
        {
          yield return player.ToNwObject<NwPlayer>();
        }
      }
    }

    public void SendMessageToAllDMs(string message, Color color)
    {
      NWScript.SendMessageToAllDMs(message.ColorString(color));
    }

    public void SendMessageToAllDMs(string message)
    {
      NWScript.SendMessageToAllDMs(message);
    }
  }
}