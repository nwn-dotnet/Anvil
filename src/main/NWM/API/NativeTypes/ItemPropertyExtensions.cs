using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public static class ItemPropertyExtensions
  {
    public static ItemPropertyType GetPropertyType(this ItemProperty itemProperty) => (ItemPropertyType) NWScript.GetItemPropertyType(itemProperty);
    public static int GetSubType(this ItemProperty itemProperty) => NWScript.GetItemPropertySubType(itemProperty);
    public static EffectDuration GetDurationType(this ItemProperty itemProperty) => (EffectDuration) NWScript.GetItemPropertyDurationType(itemProperty);
    public static float GetRemainingDuration(this ItemProperty itemProperty) => NWScript.GetItemPropertyDurationRemaining(itemProperty);
    public static float GetTotalDuration(this ItemProperty itemProperty) => NWScript.GetItemPropertyDuration(itemProperty);
    public static bool IsValid(this ItemProperty itemProperty) => NWScript.GetIsItemPropertyValid(itemProperty).ToBool();
  }
}