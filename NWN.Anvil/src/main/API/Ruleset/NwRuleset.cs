using System.Collections.Generic;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// Ruleset definitions for the module.<br/>
  /// Classes/Feats/Races/Skills/BaseItems/Spells.
  /// </summary>
  public static class NwRuleset
  {
    /// <summary>
    /// Gets a list of all classes defined in the module's ruleset.
    /// </summary>
    public static IReadOnlyList<NwClass> Classes { get; private set; }

    /// <summary>
    /// Gets a list of all feats defined in the module's ruleset.
    /// </summary>
    public static IReadOnlyList<NwFeat> Feats { get; private set; }

    /// <summary>
    /// Gets a list of all races defined in the module's ruleset.
    /// </summary>
    public static IReadOnlyList<NwRace> Races { get; private set; }

    /// <summary>
    /// Gets a list of all skills defined in the module's ruleset.
    /// </summary>
    public static IReadOnlyList<NwSkill> Skills { get; private set; }

    /// <summary>
    /// Gets a list of all base item types defined in the module's ruleset.
    /// </summary>
    public static IReadOnlyList<NwBaseItem> BaseItems { get; private set; }

    /// <summary>
    /// Gets a list of all spells defined in the module's ruleset.
    /// </summary>
    public static IReadOnlyList<NwSpell> Spells { get; private set; }

    [ServiceBinding(typeof(Factory))]
    [ServiceBindingOptions(InternalBindingPriority.API)]
    internal sealed unsafe class Factory
    {
      private readonly FunctionHook<ReloadAllHook> reloadAllHook;

      public Factory(HookService hookService)
      {
        reloadAllHook = hookService.RequestHook<ReloadAllHook>(OnReloadAll, FunctionsLinux._ZN8CNWRules9ReloadAllEv, HookOrder.Latest);
        LoadRules();
      }

      private delegate void ReloadAllHook(void* pRules);

      private static IReadOnlyList<NwClass> LoadClasses(CNWClassArray classArray, int count)
      {
        NwClass[] retVal = new NwClass[count];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new NwClass(classArray.GetItem(i), (ClassType)i);
        }

        return retVal;
      }

      private static IReadOnlyList<NwFeat> LoadFeats(CNWFeatArray featArray, int count)
      {
        NwFeat[] retVal = new NwFeat[count];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new NwFeat((Feat)i, featArray.GetItem(i));
        }

        return retVal;
      }

      private static IReadOnlyList<NwRace> LoadRaces(CNWRaceArray raceArray, int count)
      {
        NwRace[] retVal = new NwRace[count];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new NwRace((RacialType)i, raceArray.GetItem(i));
        }

        return retVal;
      }

      private static IReadOnlyList<NwSkill> LoadSkills(CNWSkillArray skillArray, int count)
      {
        NwSkill[] retVal = new NwSkill[count];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new NwSkill((Skill)i, skillArray.GetItem(i));
        }

        return retVal;
      }

      private static IReadOnlyList<NwBaseItem> LoadBaseItems(CNWBaseItemArray baseItemArray)
      {
        NwBaseItem[] retVal = new NwBaseItem[baseItemArray.m_nNumBaseItems];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new NwBaseItem(baseItemArray.GetBaseItem(i), (BaseItemType)i);
        }

        return retVal;
      }

      private static IReadOnlyList<NwSpell> LoadSpells(CNWSpellArray spellArray)
      {
        NwSpell[] retVal = new NwSpell[spellArray.m_nNumSpells];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new NwSpell(spellArray.GetSpell(i), (Spell)i);
        }

        return retVal;
      }

      private static void LoadRules()
      {
        CNWRules rules = NWNXLib.Rules();

        Races = LoadRaces(CNWRaceArray.FromPointer(rules.m_lstRaces), rules.m_nNumRaces);
        Classes = LoadClasses(CNWClassArray.FromPointer(rules.m_lstClasses), rules.m_nNumClasses);
        Skills = LoadSkills(CNWSkillArray.FromPointer(rules.m_lstSkills), rules.m_nNumSkills);
        Feats = LoadFeats(CNWFeatArray.FromPointer(rules.m_lstFeats), rules.m_nNumFeats);
        BaseItems = LoadBaseItems(rules.m_pBaseItemArray);
        Spells = LoadSpells(rules.m_pSpellArray);
      }

      private void OnReloadAll(void* pRules)
      {
        reloadAllHook.CallOriginal(pRules);
        LoadRules();
      }
    }
  }
}
