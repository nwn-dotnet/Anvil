using System;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public sealed partial class Talent
  {
    /// <summary>
    /// Gets the associated spell, if this talent is a spell.
    /// </summary>
    public Spell Spell
    {
      get => (Spell)TryGetId(TalentType.Spell);
    }

    /// <summary>
    /// Gets the associated feat, if this talent is a feat.
    /// </summary>
    public Feat Feat
    {
      get => (Feat)TryGetId(TalentType.Feat);
    }

    /// <summary>
    /// Gets the associated skill, if this talent is a skill.
    /// </summary>
    public Skill Skill
    {
      get => (Skill)TryGetId(TalentType.Skill);
    }

    /// <summary>
    /// Gets the type of this talent (Spell/Feat/Skill).
    /// </summary>
    public TalentType Type
    {
      get => (TalentType)NWScript.GetTypeFromTalent(this);
    }

    /// <summary>
    /// Gets a value indicating whether this talent is valid.
    /// </summary>
    public bool Valid
    {
      get => NWScript.GetIsTalentValid(this).ToBool();
    }

    private int TryGetId(TalentType expectedType)
    {
      if (Type != expectedType)
      {
        throw new Exception($"Expected talent to be {expectedType}, but it is {Type}!");
      }

      return NWScript.GetIdFromTalent(this);
    }
  }
}
