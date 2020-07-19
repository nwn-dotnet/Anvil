using System;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public partial class ItemProperty
  {
    /// <summary>
    /// Gets the type of this item property (as defined in itempropdef.2da)
    /// </summary>
    public ItemPropertyType PropertyType => (ItemPropertyType) NWScript.GetItemPropertyType(this);

    /// <summary>
    /// Gets the SubType index for this item property.<br/>
    /// The mapping of this value can be found by first finding the 2da name in itempropdef.2da under the "SubTypeResRef" column, then locating the index of the subtype in the specified 2da.
    /// </summary>
    public int SubType => NWScript.GetItemPropertySubType(this);

    /// <summary>
    /// Gets whether this item property is a permanent or temporary effect.
    /// </summary>
    public EffectDuration DurationType => (EffectDuration) NWScript.GetItemPropertyDurationType(this);

    /// <summary>
    /// If this item property is temporary, gets the remaining duration until the item property expires. Otherwise, returns 0.
    /// </summary>
    public TimeSpan RemainingDuration => TimeSpan.FromSeconds(NWScript.GetItemPropertyDurationRemaining(this));

    /// <summary>
    /// If this item property is temporary, gets the total duration of the item property effect. Otherwise, returns 0.
    /// </summary>
    public TimeSpan TotalDuration => TimeSpan.FromSeconds(NWScript.GetItemPropertyDuration(this));

    /// <summary>
    /// Gets if this item property is valid.
    /// </summary>
    public bool Valid => NWScript.GetIsItemPropertyValid(this).ToBool();
  }
}