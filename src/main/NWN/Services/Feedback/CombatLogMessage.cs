namespace NWN.Services
{
   public enum CombatLogMessage
   {
      /// <summary>
      /// &lt;charname&gt; : &lt;adjective described by strref&gt;.
      /// </summary>
      SimpleAdjective = 1,

      /// <summary>
      /// &lt;charname&gt; damaged : &lt;amount&gt;.
      /// </summary>
      SimpleDamage = 2,

      /// <summary>
      /// &lt;charname&gt; damages &lt;charname&gt; : &lt;amount&gt;.
      /// </summary>
      ComplexDamage = 3,

      /// <summary>
      /// &lt;charname&gt; killed &lt;charname&gt;.
      /// </summary>
      ComplexDeath = 4,

      /// <summary>
      /// &lt;charname&gt; attacks &lt;charname&gt; : *hit* / *miss* / *parried* : (&lt;attack roll&gt; + &lt;attack mod&gt; = &lt;modified total&gt;).
      /// </summary>
      ComplexAttack = 5,

      /// <summary>
      /// &lt;charname&gt; attempts &lt;special attack&gt; on &lt;charname&gt; : *success* / *failure* : (&lt;attack roll&gt; + &lt;attack mod&gt; = &lt;modified roll&gt;).
      /// </summary>
      SpecialAttack = 6,

      /// <summary>
      /// &lt;charname&gt; : &lt;saving throw type&gt; : *success* / *failure* : (&lt;saving throw roll&gt; + &lt;saving throw modifier&gt; = &lt;modified total&gt;).
      /// </summary>
      SavingThrow = 7,

      /// <summary>
      /// &lt;charname&gt; casts &lt;spell name&gt; : Spellcraft check *failure* / *success*.
      /// </summary>
      CastSpell = 8,

      /// <summary>
      /// &lt;charname&gt; : &lt;skill name&gt; : *success* / *failure* : (&lt;skill roll&gt; + &lt;skill modifier&gt; = &lt;modified total&gt; vs &lt;DC&gt; ).
      /// </summary>
      UseSkill = 9,

      /// <summary>
      /// &lt;charname&gt; : Spell Resistance &lt;SR value&gt; : *success* / *failure*.
      /// </summary>
      SpellResistance = 10,

      /// <summary>
      /// Feedback: Reason skill/feat/ability failed.<br/>
      /// NOTE: This hides ALL feedback messages, to hide individual messages use NWNX_Feedback_SetFeedbackMessageHidden().
      /// </summary>
      Feedback = 11,

      /// <summary>
      /// &lt;charname&gt; casts &lt;spell name&gt; : *spell countered by* : &lt;charname&gt; casting &lt;spell name&gt;.
      /// </summary>
      Counterspell = 12,

      /// <summary>
      /// &lt;charname&gt; attempts &lt;melee/ranged touch attack&gt; on &lt;charname&gt; : *hit/miss/critical* : (&lt;attack roll&gt; + &lt;attack mod&gt; = &lt;modified roll&gt;).
      /// </summary>
      Touchattack = 13,

      /// <summary>
      /// &lt;charname&gt; : Initiative Roll : &lt;total&gt; : (&lt;roll&gt; + &lt;modifier&gt; = &lt;total&gt;).
      /// </summary>
      Initiative = 14,

      /// <summary>
      /// Dispel Magic : &lt;charname&gt; : &lt;spell name&gt;, &lt;spell name&gt;, &lt;spell name&gt;...
      /// </summary>
      DispelMagic = 15,

      /// <summary>
      /// Unused.
      /// </summary>
      Polymorph = 17,

      /// <summary>
      /// Unused.
      /// </summary>
      Feedbackstring = 18,

      /// <summary>
      /// Unused.
      /// </summary>
      Vibrate = 19,

      /// <summary>
      /// Unused.
      /// </summary>
      Unlockachievement = 20,
   }
}
