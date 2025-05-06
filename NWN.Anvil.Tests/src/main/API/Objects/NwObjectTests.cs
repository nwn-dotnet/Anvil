using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;
using NWN.Core;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class NwObjectTests
  {
    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Tests if assigning an action/closure using WaitForObjectContext correctly updates the script context.")]
    [Timeout(2000)]
    public async Task WaitForObjectContextEntersCorrectContext()
    {
      NwModule module = NwModule.Instance;

      await module.WaitForObjectContext();

      Assert.That(NWScript.OBJECT_SELF.ToNwObject(), Is.EqualTo(module));

      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation);
      Assert.That(creature, Is.Not.Null);

      createdTestObjects.Add(creature!);

      await creature!.WaitForObjectContext();

      Assert.That(NWScript.OBJECT_SELF.ToNwObject(), Is.EqualTo(creature));
    }

    [Test(Description = "Tests if adding an action correctly queues an action on the game object.")]
    [Timeout(2000)]
    public async Task QueueCreatureActionIsQueued()
    {
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation);
      Assert.That(creature, Is.Not.Null);

      createdTestObjects.Add(creature!);

      bool actionExecuted = false;
      await creature!.AddActionToQueue(() =>
      {
        actionExecuted = true;
      });

      await NwTask.WaitUntil(() => actionExecuted);
      Assert.That(actionExecuted, Is.EqualTo(true));
    }

    [Test(Description = "Tests if assigning a valid event script correctly updates the event script.")]
    [TestCase("my_event")]
    [TestCase("")]
    [TestCase(null)]
    public void SetValidEventScriptCorrectlyUpdatesEventScript(string? script)
    {
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation)!;
      Assert.That(creature, Is.Not.Null);

      createdTestObjects.Add(creature);

      creature.SetEventScript(EventScriptType.CreatureOnSpawnIn, script);

      Assert.That(creature.GetEventScript(EventScriptType.CreatureOnSpawnIn), script == null ? Is.EqualTo(string.Empty) : Is.EqualTo(script));
    }

    [Test(Description = "Tests if assigning an invalid event script correctly throws an exception")]
    [TestCase("reallylongscriptname")]
    [TestCase("@&^/*7")]
    [TestCase(ScriptConstants.GameEventScriptName)]
    [TestCase(ScriptConstants.NWNXEventScriptName)]
    public void SetInvalidEventScriptCorrectlyThrowsException(string? script)
    {
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation)!;
      Assert.That(creature, Is.Not.Null);

      createdTestObjects.Add(creature);

      Assert.Throws<ArgumentOutOfRangeException>(() =>
      {
        creature.SetEventScript(EventScriptType.CreatureOnSpawnIn, script);
      });
    }

    [Test(Description = "Tests if attempting to set an event script after subscribing correctly throws an exception.")]
    public void SetEventScriptAfterSubscribeCorrectlyThrowsException()
    {
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation)!;
      Assert.That(creature, Is.Not.Null);

      createdTestObjects.Add(creature);
      creature.OnSpawn += _ => {};

      Assert.Throws<InvalidOperationException>(() =>
      {
        creature.SetEventScript(EventScriptType.CreatureOnSpawnIn, null);
      });
    }

    [Test(Description = "Tests if attempting to set an invalid event type correctly throws an exception.")]
    public void SetInvalidEventScriptCorrectlyThrowsException()
    {
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation)!;
      Assert.That(creature, Is.Not.Null);

      createdTestObjects.Add(creature);

      Assert.Throws<InvalidOperationException>(() =>
      {
        creature.SetEventScript(EventScriptType.ModuleOnClientEnter, null);
      });
    }

    [Test]
    [TestCase(PlayerLanguage.English, Gender.Male, "English male loc string")]
    [TestCase(PlayerLanguage.German, Gender.Male, "German male loc string")]
    [TestCase(PlayerLanguage.French, Gender.Male, "French male loc string")]
    [TestCase(PlayerLanguage.Polish, Gender.Male, "Polish male loc string")]
    [TestCase(PlayerLanguage.Italian, Gender.Male, "Italian male loc string")]
    [TestCase(PlayerLanguage.Spanish, Gender.Male, "Spanish male loc string")]
    [TestCase(PlayerLanguage.English, Gender.Female, "English female loc string")]
    [TestCase(PlayerLanguage.German, Gender.Female, "German female loc string")]
    [TestCase(PlayerLanguage.French, Gender.Female, "French female loc string")]
    [TestCase(PlayerLanguage.Polish, Gender.Female, "Polish female loc string")]
    [TestCase(PlayerLanguage.Italian, Gender.Female, "Italian female loc string")]
    [TestCase(PlayerLanguage.Spanish, Gender.Female, "Spanish female loc string")]
    public void SetLocalizedStringTest(PlayerLanguage language, Gender gender, string expectedString)
    {
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation)!;
      Assert.That(creature, Is.Not.Null);

      createdTestObjects.Add(creature);

      creature.SetLocalizedName(expectedString, language, gender);
      Assert.That(creature.GetLocalizedName(language, gender), Is.EqualTo(expectedString));
    }

    [TearDown]
    public void CleanupTestObjects()
    {
      foreach (NwGameObject testObject in createdTestObjects)
      {
        testObject.PlotFlag = false;
        testObject.Destroy();
      }

      createdTestObjects.Clear();
    }
  }
}
