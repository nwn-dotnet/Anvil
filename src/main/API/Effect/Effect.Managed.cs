// ReSharper disable once CheckNamespace

using System;
using NWM.API.Constants;

namespace NWN
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

    public static Effect CutsceneDominated() => NWScript.EffectCutsceneDominated();
    public static Effect CutsceneImmobilize() => NWScript.EffectCutsceneImmobilize();
  }
}