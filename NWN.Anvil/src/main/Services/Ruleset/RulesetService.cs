using System.Collections.Generic;
using Anvil.API;
using NWN.Native.API;
using ClassType = Anvil.API.ClassType;
using Feat = Anvil.API.Feat;
using RacialType = Anvil.API.RacialType;
using Skill = Anvil.API.Skill;

namespace Anvil.Services
{
  [ServiceBinding(typeof(RulesetService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  public sealed unsafe class RulesetService
  {
    private readonly FunctionHook<ReloadAllHook> reloadAllHook;

    public RulesetService(HookService hookService)
    {
      reloadAllHook = hookService.RequestHook<ReloadAllHook>(OnReloadAll, FunctionsLinux._ZN8CNWRules9ReloadAllEv, HookOrder.Latest);
      LoadRules();
    }

    private delegate void ReloadAllHook(void* pRules);

    /// <summary>
    /// Gets a list of all classes defined in the module's ruleset.
    /// </summary>
    public IReadOnlyList<NwClass> Classes { get; private set; }

    /// <summary>
    /// Gets a list of all feats defined in the module's ruleset.
    /// </summary>
    public IReadOnlyList<NwFeat> Feats { get; private set; }

    /// <summary>
    /// Gets a list of all races defined in the module's ruleset.
    /// </summary>
    public IReadOnlyList<NwRace> Races { get; private set; }

    /// <summary>
    /// Gets a list of all skills defined in the module's ruleset.
    /// </summary>
    public IReadOnlyList<NwSkill> Skills { get; private set; }

    private static IReadOnlyList<NwClass> LoadClasses(CNWClassArray classes, int count)
    {
      NwClass[] retVal = new NwClass[count];
      for (int i = 0; i < count; i++)
      {
        retVal[i] = new NwClass((ClassType)i, classes.GetItem(i));
      }

      return retVal;
    }

    private static IReadOnlyList<NwFeat> LoadFeats(CNWFeatArray feats, int count)
    {
      NwFeat[] retVal = new NwFeat[count];
      for (int i = 0; i < count; i++)
      {
        retVal[i] = new NwFeat((Feat)i, feats.GetItem(i));
      }

      return retVal;
    }

    private static IReadOnlyList<NwRace> LoadRaces(CNWRaceArray races, int count)
    {
      NwRace[] retVal = new NwRace[count];
      for (int i = 0; i < count; i++)
      {
        retVal[i] = new NwRace((RacialType)i, races.GetItem(i));
      }

      return retVal;
    }

    private static IReadOnlyList<NwSkill> LoadSkills(CNWSkillArray skills, int count)
    {
      NwSkill[] retVal = new NwSkill[count];
      for (int i = 0; i < count; i++)
      {
        retVal[i] = new NwSkill((Skill)i, skills.GetItem(i));
      }

      return retVal;
    }

    private void LoadRules()
    {
      CNWRules rules = NWNXLib.Rules();

      Races = LoadRaces(CNWRaceArray.FromPointer(rules.m_lstRaces), rules.m_nNumRaces);
      Classes = LoadClasses(CNWClassArray.FromPointer(rules.m_lstClasses), rules.m_nNumClasses);
      Skills = LoadSkills(CNWSkillArray.FromPointer(rules.m_lstSkills), rules.m_nNumSkills);
      Feats = LoadFeats(CNWFeatArray.FromPointer(rules.m_lstFeats), rules.m_nNumFeats);
    }

    private void OnReloadAll(void* pRules)
    {
      reloadAllHook.CallOriginal(pRules);
      LoadRules();
    }
  }
}
