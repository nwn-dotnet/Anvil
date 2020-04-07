using System.Collections.Generic;
using NWM.Core;
using NWN;

namespace NWM.API
{
  [Service]
  public sealed class ObjectManager
  {
    public NwModule Module => NwObject.Module;

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
  }
}