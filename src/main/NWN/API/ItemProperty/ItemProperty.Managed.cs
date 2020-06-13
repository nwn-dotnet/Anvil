using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public partial class ItemProperty
  {
    public ItemPropertyType PropertyType => (ItemPropertyType) NWScript.GetItemPropertyType(this);
    public int SubType => NWScript.GetItemPropertySubType(this);
    public EffectDuration DurationType => (EffectDuration) NWScript.GetItemPropertyDurationType(this);
    public float RemainingDuration => NWScript.GetItemPropertyDurationRemaining(this);
    public float TotalDuration => NWScript.GetItemPropertyDuration(this);

    public bool Valid => NWScript.GetIsItemPropertyValid(this).ToBool();
  }
}