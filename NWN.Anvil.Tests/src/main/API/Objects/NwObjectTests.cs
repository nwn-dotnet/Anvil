using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;
using NWN.Core;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwObjectTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

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
