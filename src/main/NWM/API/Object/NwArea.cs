using System.Collections.Generic;
using System.Numerics;
using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public sealed class NwArea : NwObject
  {
    internal NwArea(uint objectId) : base(objectId) {}

    /// <summary>
    ///  Returns true if this area is above ground, or false if it is underground
    /// </summary>
    public bool IsAboveGround => (AreaInfo) NWScript.GetIsAreaAboveGround(this) == AreaInfo.AboveGround;

    /// <summary>
    ///  Returns true if this area is natural, or false if it is artificial.
    /// </summary>
    public bool IsNatural => (AreaInfo) NWScript.GetIsAreaNatural(this) == AreaInfo.Natural;

    /// <summary>
    ///  Gets the size of the area
    /// <returns>The number of tiles that the area is wide/high.</returns>
    /// </summary>
    public Vector2 AreaSize => new Vector2(NWScript.GetAreaSize((int) AreaSizeDimension.Width, this), NWScript.GetAreaSize((int) AreaSizeDimension.Height, this));

    public IEnumerable<NwObject> AreaObjects
    {
      get
      {
        for (NwObject areaObj = NWScript.GetFirstObjectInArea(this).ToNwObject(); areaObj != null; areaObj = NWScript.GetNextObjectInArea(this).ToNwObject())
        {
          yield return areaObj;
        }
      }
    }
  }
}