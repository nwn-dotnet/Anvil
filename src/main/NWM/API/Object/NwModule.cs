using System.Collections.Generic;
using NWM.Core;
using NWN;

namespace NWM.API
{
  public sealed class NwModule : NwObject
  {
    internal NwModule(uint objectId) : base(objectId) {}

    public static readonly NwModule Instance = new NwModule(NWScript.GetModule());

    public IEnumerable<NwArea> Areas
    {
      get
      {
        for (NwArea area = NWScript.GetFirstArea().ToNwObject<NwArea>(); area != null; area = NWScript.GetNextArea().ToNwObject<NwArea>())
        {
          yield return area;
        }
      }
    }

    public IEnumerable<NwGameObject> GetObjectsByTag(string tag)
    {
      int i;
      NwGameObject obj;
      for (i = 0, obj = NWScript.GetObjectByTag(tag, i).ToNwObject<NwGameObject>(); obj != INVALID; i++, obj = NWScript.GetObjectByTag(tag, i).ToNwObject<NwGameObject>())
      {
        yield return obj;
      }
    }

    public IEnumerable<NwPlayer> Players
    {
      get
      {
        for (NwPlayer player = NWScript.GetFirstPC().ToNwObject<NwPlayer>(); player != null; player = NWScript.GetNextPC().ToNwObject<NwPlayer>())
        {
          yield return player;
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