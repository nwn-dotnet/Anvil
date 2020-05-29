using NWN;

namespace NWM.API
{
  public partial class Effect
  {
    /// <summary>
    ///  Returns the string tag set for the provided effect.
    ///  - If no tag has been set, returns an empty string.
    /// </summary>
    public string Tag => NWScript.GetEffectTag(this);

    public static Effect TagEffect(Effect effect, string tag)
    {
      return NWScript.TagEffect(effect, tag);
    }
  }
}