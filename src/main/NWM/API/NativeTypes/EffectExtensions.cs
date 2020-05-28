// ReSharper disable once CheckNamespace
namespace NWN
{
  public static class EffectExtensions
  {
    /// <summary>
    ///  Returns the string tag set for the provided effect.
    ///  - If no tag has been set, returns an empty string.
    /// </summary>
    public static string GetTag(this Effect effect)
    {
      return NWScript.GetEffectTag(effect);
    }

    public static Effect TagEffect(Effect effect, string tag)
    {
      return NWScript.TagEffect(effect, tag);
    }
  }
}