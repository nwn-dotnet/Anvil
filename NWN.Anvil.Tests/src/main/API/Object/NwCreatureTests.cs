using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwCreatureTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "Creating a creature with a valid ResRef creates a valid creature.")]
    [TestCase(StandardResRef.Creature.nw_bandit001)]
    [TestCase(StandardResRef.Creature.nw_bandit002)]
    [TestCase(StandardResRef.Creature.nw_bandit003)]
    [TestCase(StandardResRef.Creature.nw_shopkeep)]
    [TestCase(StandardResRef.Creature.nw_bartender)]
    [TestCase(StandardResRef.Creature.nw_drgblack001)]
    [TestCase(StandardResRef.Creature.nw_drgblue002)]
    [TestCase(StandardResRef.Creature.nw_drggold003)]
    [TestCase(StandardResRef.Creature.nw_drgred003)]
    [TestCase(StandardResRef.Creature.x2_beholder001)]
    public void CreateCreatureIsCreated(string creatureResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(creatureResRef, startLocation);

      Assert.That(creature, Is.Not.Null, $"Creature {creatureResRef} was null after creation.");
      Assert.That(creature!.IsValid, Is.True, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);
    }

    [Test(Description = "Cloning a creature with copyLocalState = true copies expected local state information.")]
    [TestCase(StandardResRef.Creature.nw_bandit001)]
    [TestCase(StandardResRef.Creature.nw_bandit002)]
    [TestCase(StandardResRef.Creature.nw_bandit003)]
    [TestCase(StandardResRef.Creature.nw_shopkeep)]
    [TestCase(StandardResRef.Creature.nw_bartender)]
    [TestCase(StandardResRef.Creature.nw_drgblack001)]
    [TestCase(StandardResRef.Creature.nw_drgblue002)]
    [TestCase(StandardResRef.Creature.nw_drggold003)]
    [TestCase(StandardResRef.Creature.nw_drgred003)]
    [TestCase(StandardResRef.Creature.x2_beholder001)]
    public void CloneCreatureWithLocalStateIsCopied(string creatureResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(creatureResRef, startLocation);

      Assert.That(creature, Is.Not.Null, $"Creature {creatureResRef} was null after creation.");
      Assert.That(creature!.IsValid, Is.True, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);

      LocalVariableInt testVar = creature.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwCreature clone = creature.Clone(startLocation);

      Assert.That(clone, Is.Not.Null, $"Creature {creatureResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Creature {creatureResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.True, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.That(cloneTestVar.Value, Is.EqualTo(testVar.Value), "Local variable on the cloned creature did not match the value of the original creature.");
    }

    [Test(Description = "Cloning a creature with copyLocalState = false does not copy local state information.")]
    [TestCase(StandardResRef.Creature.nw_bandit001)]
    [TestCase(StandardResRef.Creature.nw_bandit002)]
    [TestCase(StandardResRef.Creature.nw_bandit003)]
    [TestCase(StandardResRef.Creature.nw_shopkeep)]
    [TestCase(StandardResRef.Creature.nw_bartender)]
    [TestCase(StandardResRef.Creature.nw_drgblack001)]
    [TestCase(StandardResRef.Creature.nw_drgblue002)]
    [TestCase(StandardResRef.Creature.nw_drggold003)]
    [TestCase(StandardResRef.Creature.nw_drgred003)]
    [TestCase(StandardResRef.Creature.x2_beholder001)]
    public void CloneCreatureNoLocalStateIsNotCopied(string creatureResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(creatureResRef, startLocation);

      Assert.That(creature, Is.Not.Null, $"Creature {creatureResRef} was null after creation.");
      Assert.That(creature!.IsValid, Is.True, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);

      LocalVariableInt testVar = creature.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwCreature clone = creature.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Creature {creatureResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Creature {creatureResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.False, "Local variable exists on the clone with copyLocalState = false.");
      Assert.That(cloneTestVar.Value, Is.Not.EqualTo(testVar.Value), "Local variable on the cloned creature matches the value of the original creature.");
    }

    [Test(Description = "Cloning a creature with a custom tag defined causes the new creature to gain the new tag.")]
    [TestCase(StandardResRef.Creature.nw_bandit001)]
    [TestCase(StandardResRef.Creature.nw_bandit002)]
    [TestCase(StandardResRef.Creature.nw_bandit003)]
    [TestCase(StandardResRef.Creature.nw_shopkeep)]
    [TestCase(StandardResRef.Creature.nw_bartender)]
    [TestCase(StandardResRef.Creature.nw_drgblack001)]
    [TestCase(StandardResRef.Creature.nw_drgblue002)]
    [TestCase(StandardResRef.Creature.nw_drggold003)]
    [TestCase(StandardResRef.Creature.nw_drgred003)]
    [TestCase(StandardResRef.Creature.x2_beholder001)]
    public void CloneCreatureCustomTagIsApplied(string creatureResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(creatureResRef, startLocation);

      Assert.That(creature, Is.Not.Null, $"Creature {creatureResRef} was null after creation.");
      Assert.That(creature!.IsValid, Is.True, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);

      string expectedNewTag = "expectedNewTag";
      NwCreature clone = creature.Clone(startLocation, expectedNewTag, false);

      Assert.That(clone, Is.Not.Null, $"Creature {creatureResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Creature {creatureResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(expectedNewTag), "Tag defined in clone method was not applied to the cloned creature.");
    }

    [Test(Description = "Cloning a creature with no tag defined uses the original creature's tag instead.")]
    [TestCase(StandardResRef.Creature.nw_bandit001)]
    [TestCase(StandardResRef.Creature.nw_bandit002)]
    [TestCase(StandardResRef.Creature.nw_bandit003)]
    [TestCase(StandardResRef.Creature.nw_shopkeep)]
    [TestCase(StandardResRef.Creature.nw_bartender)]
    [TestCase(StandardResRef.Creature.nw_drgblack001)]
    [TestCase(StandardResRef.Creature.nw_drgblue002)]
    [TestCase(StandardResRef.Creature.nw_drggold003)]
    [TestCase(StandardResRef.Creature.nw_drgred003)]
    [TestCase(StandardResRef.Creature.x2_beholder001)]
    public void CloneCreatureWithoutTagOriginalTagIsCopied(string creatureResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(creatureResRef, startLocation);

      Assert.That(creature, Is.Not.Null, $"Creature {creatureResRef} was null after creation.");
      Assert.That(creature!.IsValid, Is.True, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);
      creature.Tag = "expectedNewTag";

      NwCreature clone = creature.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Creature {creatureResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Creature {creatureResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(creature.Tag), "Cloned creature's tag did not match the original creature's.");
    }

    [Test(Description = "Querying remaining feat uses returns a valid value.")]
    public void GetFeatRemainingUsesReturnsValidValue()
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.tanarukk, startLocation);

      Assert.That(creature, Is.Not.Null, "Creature was null after creation.");
      Assert.That(creature!.IsValid, Is.True, "Creature was invalid after creation.");

      createdTestObjects.Add(creature);

      NwFeat? feat = NwFeat.FromFeatType(Feat.BarbarianRage);
      Assert.That(creature.GetFeatRemainingUses(feat!), Is.EqualTo(1));
    }

    [Test(Description = "Querying total feat uses returns a valid value.")]
    public void GetFeatTotalUsesReturnsValidValue()
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.tanarukk, startLocation);

      Assert.That(creature, Is.Not.Null, "Creature was null after creation.");
      Assert.That(creature!.IsValid, Is.True, "Creature was invalid after creation.");

      createdTestObjects.Add(creature);

      NwFeat? feat = NwFeat.FromFeatType(Feat.BarbarianRage);

      Assert.That(feat, Is.Not.Null, "Could not get feat.");
      Assert.That(creature.GetFeatTotalUses(feat!), Is.EqualTo(1));
    }

    [Test(Description = "Setting remaining feat uses correctly updates the creature.")]
    [TestCase(0)]
    [TestCase(2)]
    [TestCase(5)]
    public void SetFeatRemainingUsesCorrectlyUpdatesState(byte uses)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.tanarukk, startLocation);

      Assert.That(creature, Is.Not.Null, "Creature was null after creation.");
      Assert.That(creature!.IsValid, Is.True, "Creature was invalid after creation.");

      createdTestObjects.Add(creature);
      NwFeat? feat = NwFeat.FromFeatType(Feat.BarbarianRage);

      Assert.That(feat, Is.Not.Null, "Could not get feat.");

      creature.SetFeatRemainingUses(feat!, uses);

      Assert.That(creature.GetFeatRemainingUses(feat!), Is.EqualTo(Math.Min(uses, creature.GetFeatTotalUses(feat!))), "Remaining feat uses was not updated after being set.");
      Assert.That(creature.HasFeatPrepared(feat!), Is.EqualTo(uses > 0), "Creature incorrectly assumes the feat is/is not available.");
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
