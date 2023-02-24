using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwTriggerTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "Serializing and deserializing a trigger generates valid gff data, and a new valid trigger.")]
    [TestCase(StandardResRef.Trigger.newtransition)]
    [TestCase(StandardResRef.Trigger.x0_hench_trigger)]
    [TestCase(StandardResRef.Trigger.newgeneric)]
    [TestCase(StandardResRef.Trigger.trackstrigger)]
    [TestCase(StandardResRef.Trigger.x0_saferest)]
    [TestCase(StandardResRef.Trigger.x2_inter_nr)]
    [TestCase(StandardResRef.Trigger.x2_oneliner_nr)]
    [TestCase(StandardResRef.Trigger.x2_party_r)]
    [TestCase(StandardResRef.Trigger.tr_dismount)]
    [TestCase(StandardResRef.Trigger.x0_sectrig_trhi)]
    public void SerializeTriggerCreatesValidData(string triggerResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwTrigger? trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.That(trigger, Is.Not.Null, $"Trigger {triggerResRef} was null after creation.");
      Assert.That(trigger!.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);

      byte[]? triggerData = trigger.Serialize();

      Assert.That(triggerData, Is.Not.Null);
      Assert.That(triggerData, Has.Length.GreaterThan(0));

      NwTrigger? trigger2 = NwTrigger.Deserialize(triggerData!);
      Assert.That(trigger2, Is.Not.Null);
      Assert.That(trigger2!.IsValid, Is.True);

      createdTestObjects.Add(trigger2);

      Assert.That(trigger2.Area, Is.Null);
    }

    [Test(Description = "Creating a trigger with a valid ResRef creates a valid trigger.")]
    [TestCase(StandardResRef.Trigger.newtransition)]
    [TestCase(StandardResRef.Trigger.x0_hench_trigger)]
    [TestCase(StandardResRef.Trigger.newgeneric)]
    [TestCase(StandardResRef.Trigger.trackstrigger)]
    [TestCase(StandardResRef.Trigger.x0_saferest)]
    [TestCase(StandardResRef.Trigger.x2_inter_nr)]
    [TestCase(StandardResRef.Trigger.x2_oneliner_nr)]
    [TestCase(StandardResRef.Trigger.x2_party_r)]
    [TestCase(StandardResRef.Trigger.tr_dismount)]
    [TestCase(StandardResRef.Trigger.x0_sectrig_trhi)]
    public void CreateTriggerIsCreated(string triggerResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwTrigger? trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.That(trigger, Is.Not.Null, $"Trigger {triggerResRef} was null after creation.");
      Assert.That(trigger!.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);
    }

    [Test(Description = "Cloning a trigger with copyLocalState = true copies expected local state information.")]
    [TestCase(StandardResRef.Trigger.newtransition)]
    [TestCase(StandardResRef.Trigger.x0_hench_trigger)]
    [TestCase(StandardResRef.Trigger.newgeneric)]
    [TestCase(StandardResRef.Trigger.trackstrigger)]
    [TestCase(StandardResRef.Trigger.x0_saferest)]
    [TestCase(StandardResRef.Trigger.x2_inter_nr)]
    [TestCase(StandardResRef.Trigger.x2_oneliner_nr)]
    [TestCase(StandardResRef.Trigger.x2_party_r)]
    [TestCase(StandardResRef.Trigger.tr_dismount)]
    [TestCase(StandardResRef.Trigger.x0_sectrig_trhi)]
    public void CloneTriggerWithLocalStateIsCopied(string triggerResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwTrigger? trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.That(trigger, Is.Not.Null, $"Trigger {triggerResRef} was null after creation.");
      Assert.That(trigger!.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);

      LocalVariableInt testVar = trigger.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwTrigger clone = trigger.Clone(startLocation);

      Assert.That(clone, Is.Not.Null, $"Trigger {triggerResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.True, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.That(cloneTestVar.Value, Is.EqualTo(testVar.Value), "Local variable on the cloned trigger did not match the value of the original trigger.");
    }

    [Test(Description = "Cloning a trigger with copyLocalState = false does not copy local state information.")]
    [TestCase(StandardResRef.Trigger.newtransition)]
    [TestCase(StandardResRef.Trigger.x0_hench_trigger)]
    [TestCase(StandardResRef.Trigger.newgeneric)]
    [TestCase(StandardResRef.Trigger.trackstrigger)]
    [TestCase(StandardResRef.Trigger.x0_saferest)]
    [TestCase(StandardResRef.Trigger.x2_inter_nr)]
    [TestCase(StandardResRef.Trigger.x2_oneliner_nr)]
    [TestCase(StandardResRef.Trigger.x2_party_r)]
    [TestCase(StandardResRef.Trigger.tr_dismount)]
    [TestCase(StandardResRef.Trigger.x0_sectrig_trhi)]
    public void CloneTriggerNoLocalStateIsNotCopied(string triggerResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwTrigger? trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.That(trigger, Is.Not.Null, $"Trigger {triggerResRef} was null after creation.");
      Assert.That(trigger!.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);

      LocalVariableInt testVar = trigger.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwTrigger clone = trigger.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Trigger {triggerResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.False, "Local variable exists on the clone with copyLocalState = false.");
      Assert.That(cloneTestVar.Value, Is.Not.EqualTo(testVar.Value), "Local variable on the cloned trigger matches the value of the original trigger.");
    }

    [Test(Description = "Cloning a trigger with a custom tag defined causes the new trigger to gain the new tag.")]
    [TestCase(StandardResRef.Trigger.newtransition)]
    [TestCase(StandardResRef.Trigger.x0_hench_trigger)]
    [TestCase(StandardResRef.Trigger.newgeneric)]
    [TestCase(StandardResRef.Trigger.trackstrigger)]
    [TestCase(StandardResRef.Trigger.x0_saferest)]
    [TestCase(StandardResRef.Trigger.x2_inter_nr)]
    [TestCase(StandardResRef.Trigger.x2_oneliner_nr)]
    [TestCase(StandardResRef.Trigger.x2_party_r)]
    [TestCase(StandardResRef.Trigger.tr_dismount)]
    [TestCase(StandardResRef.Trigger.x0_sectrig_trhi)]
    public void CloneTriggerCustomTagIsApplied(string triggerResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwTrigger? trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.That(trigger, Is.Not.Null, $"Trigger {triggerResRef} was null after creation.");
      Assert.That(trigger!.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);

      string expectedNewTag = "expectedNewTag";
      NwTrigger clone = trigger.Clone(startLocation, expectedNewTag, false);

      Assert.That(clone, Is.Not.Null, $"Trigger {triggerResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(expectedNewTag), "Tag defined in clone method was not applied to the cloned trigger.");
    }

    [Test(Description = "Cloning a trigger with no tag defined uses the original trigger's tag instead.")]
    [TestCase(StandardResRef.Trigger.newtransition)]
    [TestCase(StandardResRef.Trigger.x0_hench_trigger)]
    [TestCase(StandardResRef.Trigger.newgeneric)]
    [TestCase(StandardResRef.Trigger.trackstrigger)]
    [TestCase(StandardResRef.Trigger.x0_saferest)]
    [TestCase(StandardResRef.Trigger.x2_inter_nr)]
    [TestCase(StandardResRef.Trigger.x2_oneliner_nr)]
    [TestCase(StandardResRef.Trigger.x2_party_r)]
    [TestCase(StandardResRef.Trigger.tr_dismount)]
    [TestCase(StandardResRef.Trigger.x0_sectrig_trhi)]
    public void CloneTriggerWithoutTagOriginalTagIsCopied(string triggerResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwTrigger? trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.That(trigger, Is.Not.Null, $"Trigger {triggerResRef} was null after creation.");
      Assert.That(trigger!.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);
      trigger.Tag = "expectedNewTag";

      NwTrigger clone = trigger.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Trigger {triggerResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Trigger {triggerResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(trigger.Tag), "Cloned trigger's tag did not match the original trigger's.");
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
