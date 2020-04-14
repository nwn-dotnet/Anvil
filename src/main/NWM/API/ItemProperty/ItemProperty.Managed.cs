// ReSharper disable once CheckNamespace

using System;
using NWM.API;

namespace NWN
{
  public partial class ItemProperty
  {
    public NWM.API.Constants.ItemProperty PropertyType => (NWM.API.Constants.ItemProperty) NWScript.GetItemPropertyType(this);
    public int SubType => NWScript.GetItemPropertySubType(this);
    public EffectDuration DurationType => (EffectDuration) NWScript.GetItemPropertyDurationType(this);
    public float RemainingDuration => NWScript.GetItemPropertyDurationRemaining(this);
    public float TotalDuration => NWScript.GetItemPropertyDuration(this);

    public bool Valid => NWScript.GetIsItemPropertyValid(this).ToBool();
  }
}