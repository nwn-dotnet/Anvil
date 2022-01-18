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
      NwTrigger trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.IsNotNull(trigger, $"Trigger {triggerResRef} was null after creation.");
      Assert.IsTrue(trigger.IsValid, $"Trigger {triggerResRef} was invalid after creation.");

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
      NwTrigger trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.IsNotNull(trigger, $"Trigger {triggerResRef} was null after creation.");
      Assert.IsTrue(trigger.IsValid, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);

      LocalVariableInt testVar = trigger.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwTrigger clone = trigger.Clone(startLocation);

      Assert.IsNotNull(clone, $"Trigger {triggerResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Trigger {triggerResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.IsTrue(cloneTestVar.HasValue, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.AreEqual(cloneTestVar.Value, testVar.Value, "Local variable on the cloned trigger did not match the value of the original trigger.");
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
      NwTrigger trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.IsNotNull(trigger, $"Trigger {triggerResRef} was null after creation.");
      Assert.IsTrue(trigger.IsValid, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);

      LocalVariableInt testVar = trigger.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwTrigger clone = trigger.Clone(startLocation, null, false);

      Assert.IsNotNull(clone, $"Trigger {triggerResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Trigger {triggerResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.IsFalse(cloneTestVar.HasValue, "Local variable exists on the clone with copyLocalState = false.");
      Assert.AreNotEqual(cloneTestVar.Value, testVar.Value, "Local variable on the cloned trigger matches the value of the original trigger.");
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
      NwTrigger trigger = NwTrigger.Create(triggerResRef, startLocation);

      Assert.IsNotNull(trigger, $"Trigger {triggerResRef} was null after creation.");
      Assert.IsTrue(trigger.IsValid, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);

      string expectedNewTag = "expectedNewTag";
      NwTrigger clone = trigger.Clone(startLocation, expectedNewTag, false);

      Assert.IsNotNull(clone, $"Trigger {triggerResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Trigger {triggerResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.AreEqual(expectedNewTag, clone.Tag, "Tag defined in clone method was not applied to the cloned trigger.");
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
      NwTrigger trigger = NwTrigger.Create(triggerResRef, startLocation);
      trigger.Tag = "expectedNewTag";

      Assert.IsNotNull(trigger, $"Trigger {triggerResRef} was null after creation.");
      Assert.IsTrue(trigger.IsValid, $"Trigger {triggerResRef} was invalid after creation.");

      createdTestObjects.Add(trigger);

      NwTrigger clone = trigger.Clone(startLocation, null, false);

      Assert.IsNotNull(clone, $"Trigger {triggerResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Trigger {triggerResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.AreEqual(trigger.Tag, clone.Tag, "Cloned trigger's tag did not match the original trigger's.");
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
