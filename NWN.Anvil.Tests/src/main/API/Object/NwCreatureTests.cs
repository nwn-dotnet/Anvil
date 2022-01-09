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
      NwCreature creature = NwCreature.Create(creatureResRef, startLocation);

      Assert.IsNotNull(creature, $"Creature {creatureResRef} was null after creation.");
      Assert.IsTrue(creature.IsValid, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);

      LocalVariableInt testVar = creature.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwCreature clone = creature.Clone(startLocation, null, true);

      Assert.IsNotNull(clone, $"Creature {creatureResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Creature {creatureResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.IsTrue(cloneTestVar.HasValue, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.AreEqual(cloneTestVar.Value, testVar.Value, "Local variable on the cloned creature did not match the value of the original creature.");
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
      NwCreature creature = NwCreature.Create(creatureResRef, startLocation);

      Assert.IsNotNull(creature, $"Creature {creatureResRef} was null after creation.");
      Assert.IsTrue(creature.IsValid, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);

      LocalVariableInt testVar = creature.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwCreature clone = creature.Clone(startLocation, null, false);

      Assert.IsNotNull(clone, $"Creature {creatureResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Creature {creatureResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.IsFalse(cloneTestVar.HasValue, "Local variable exists on the clone with copyLocalState = false.");
      Assert.AreNotEqual(cloneTestVar.Value, testVar.Value, "Local variable on the cloned creature matches the value of the original creature.");
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
      NwCreature creature = NwCreature.Create(creatureResRef, startLocation);

      Assert.IsNotNull(creature, $"Creature {creatureResRef} was null after creation.");
      Assert.IsTrue(creature.IsValid, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);

      string expectedNewTag = "expectedNewTag";
      NwCreature clone = creature.Clone(startLocation, expectedNewTag, false);

      Assert.IsNotNull(clone, $"Creature {creatureResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Creature {creatureResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.AreEqual(expectedNewTag, clone.Tag, "Tag defined in clone method was not applied to the cloned creature.");
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
      NwCreature creature = NwCreature.Create(creatureResRef, startLocation);
      creature.Tag = "expectedNewTag";

      Assert.IsNotNull(creature, $"Creature {creatureResRef} was null after creation.");
      Assert.IsTrue(creature.IsValid, $"Creature {creatureResRef} was invalid after creation.");

      createdTestObjects.Add(creature);

      NwCreature clone = creature.Clone(startLocation, null, false);

      Assert.IsNotNull(clone, $"Creature {creatureResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Creature {creatureResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.AreEqual(creature.Tag, clone.Tag, "Cloned creature's tag did not match the original creature's.");
    }

    [TearDown]
    public void CleanupTestObject()
    {
      foreach (NwGameObject testObject in createdTestObjects)
      {
        testObject.Destroy();
      }

      createdTestObjects.Clear();
    }
  }
}
