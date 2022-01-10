using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwStoreTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "Creating a store with a valid ResRef creates a valid store.")]
    [TestCase(StandardResRef.Store.nw_storethief001)]
    [TestCase(StandardResRef.Store.x2_merc_dye)]
    [TestCase(StandardResRef.Store.nw_storgenral001)]
    [TestCase(StandardResRef.Store.x2_storegenl001)]
    [TestCase(StandardResRef.Store.nw_storemagic001)]
    [TestCase(StandardResRef.Store.x2_storemage001)]
    [TestCase(StandardResRef.Store.nw_storenatu001)]
    [TestCase(StandardResRef.Store.nw_lostitems)]
    [TestCase(StandardResRef.Store.nw_storespec001)]
    [TestCase(StandardResRef.Store.nw_storebar01)]
    [TestCase(StandardResRef.Store.nw_storetmple001)]
    [TestCase(StandardResRef.Store.x2_genie)]
    [TestCase(StandardResRef.Store.nw_storeweap001)]
    public void CreateStoreIsCreated(string storeResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwStore store = NwStore.Create(storeResRef, startLocation);

      Assert.IsNotNull(store, $"Store {storeResRef} was null after creation.");
      Assert.IsTrue(store.IsValid, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);
    }

    [Test(Description = "Cloning a store with copyLocalState = true copies expected local state information.")]
    [TestCase(StandardResRef.Store.nw_storethief001)]
    [TestCase(StandardResRef.Store.x2_merc_dye)]
    [TestCase(StandardResRef.Store.nw_storgenral001)]
    [TestCase(StandardResRef.Store.x2_storegenl001)]
    [TestCase(StandardResRef.Store.nw_storemagic001)]
    [TestCase(StandardResRef.Store.x2_storemage001)]
    [TestCase(StandardResRef.Store.nw_storenatu001)]
    [TestCase(StandardResRef.Store.nw_lostitems)]
    [TestCase(StandardResRef.Store.nw_storespec001)]
    [TestCase(StandardResRef.Store.nw_storebar01)]
    [TestCase(StandardResRef.Store.nw_storetmple001)]
    [TestCase(StandardResRef.Store.x2_genie)]
    [TestCase(StandardResRef.Store.nw_storeweap001)]
    public void CloneStoreWithLocalStateIsCopied(string storeResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwStore store = NwStore.Create(storeResRef, startLocation);

      Assert.IsNotNull(store, $"Store {storeResRef} was null after creation.");
      Assert.IsTrue(store.IsValid, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);

      LocalVariableInt testVar = store.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwStore clone = store.Clone(startLocation);

      Assert.IsNotNull(clone, $"Store {storeResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Store {storeResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.IsTrue(cloneTestVar.HasValue, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.AreEqual(cloneTestVar.Value, testVar.Value, "Local variable on the cloned store did not match the value of the original store.");
    }

    [Test(Description = "Cloning a store with copyLocalState = false does not copy local state information.")]
    [TestCase(StandardResRef.Store.nw_storethief001)]
    [TestCase(StandardResRef.Store.x2_merc_dye)]
    [TestCase(StandardResRef.Store.nw_storgenral001)]
    [TestCase(StandardResRef.Store.x2_storegenl001)]
    [TestCase(StandardResRef.Store.nw_storemagic001)]
    [TestCase(StandardResRef.Store.x2_storemage001)]
    [TestCase(StandardResRef.Store.nw_storenatu001)]
    [TestCase(StandardResRef.Store.nw_lostitems)]
    [TestCase(StandardResRef.Store.nw_storespec001)]
    [TestCase(StandardResRef.Store.nw_storebar01)]
    [TestCase(StandardResRef.Store.nw_storetmple001)]
    [TestCase(StandardResRef.Store.x2_genie)]
    [TestCase(StandardResRef.Store.nw_storeweap001)]
    public void CloneStoreNoLocalStateIsNotCopied(string storeResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwStore store = NwStore.Create(storeResRef, startLocation);

      Assert.IsNotNull(store, $"Store {storeResRef} was null after creation.");
      Assert.IsTrue(store.IsValid, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);

      LocalVariableInt testVar = store.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwStore clone = store.Clone(startLocation, null, false);

      Assert.IsNotNull(clone, $"Store {storeResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Store {storeResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.IsFalse(cloneTestVar.HasValue, "Local variable exists on the clone with copyLocalState = false.");
      Assert.AreNotEqual(cloneTestVar.Value, testVar.Value, "Local variable on the cloned store matches the value of the original store.");
    }

    [Test(Description = "Cloning a store with a custom tag defined causes the new store to gain the new tag.")]
    [TestCase(StandardResRef.Store.nw_storethief001)]
    [TestCase(StandardResRef.Store.x2_merc_dye)]
    [TestCase(StandardResRef.Store.nw_storgenral001)]
    [TestCase(StandardResRef.Store.x2_storegenl001)]
    [TestCase(StandardResRef.Store.nw_storemagic001)]
    [TestCase(StandardResRef.Store.x2_storemage001)]
    [TestCase(StandardResRef.Store.nw_storenatu001)]
    [TestCase(StandardResRef.Store.nw_lostitems)]
    [TestCase(StandardResRef.Store.nw_storespec001)]
    [TestCase(StandardResRef.Store.nw_storebar01)]
    [TestCase(StandardResRef.Store.nw_storetmple001)]
    [TestCase(StandardResRef.Store.x2_genie)]
    [TestCase(StandardResRef.Store.nw_storeweap001)]
    public void CloneStoreCustomTagIsApplied(string storeResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwStore store = NwStore.Create(storeResRef, startLocation);

      Assert.IsNotNull(store, $"Store {storeResRef} was null after creation.");
      Assert.IsTrue(store.IsValid, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);

      string expectedNewTag = "expectedNewTag";
      NwStore clone = store.Clone(startLocation, expectedNewTag, false);

      Assert.IsNotNull(clone, $"Store {storeResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Store {storeResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.AreEqual(expectedNewTag, clone.Tag, "Tag defined in clone method was not applied to the cloned store.");
    }

    [Test(Description = "Cloning a store with no tag defined uses the original store's tag instead.")]
    [TestCase(StandardResRef.Store.nw_storethief001)]
    [TestCase(StandardResRef.Store.x2_merc_dye)]
    [TestCase(StandardResRef.Store.nw_storgenral001)]
    [TestCase(StandardResRef.Store.x2_storegenl001)]
    [TestCase(StandardResRef.Store.nw_storemagic001)]
    [TestCase(StandardResRef.Store.x2_storemage001)]
    [TestCase(StandardResRef.Store.nw_storenatu001)]
    [TestCase(StandardResRef.Store.nw_lostitems)]
    [TestCase(StandardResRef.Store.nw_storespec001)]
    [TestCase(StandardResRef.Store.nw_storebar01)]
    [TestCase(StandardResRef.Store.nw_storetmple001)]
    [TestCase(StandardResRef.Store.x2_genie)]
    [TestCase(StandardResRef.Store.nw_storeweap001)]
    public void CloneStoreWithoutTagOriginalTagIsCopied(string storeResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwStore store = NwStore.Create(storeResRef, startLocation);
      store.Tag = "expectedNewTag";

      Assert.IsNotNull(store, $"Store {storeResRef} was null after creation.");
      Assert.IsTrue(store.IsValid, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);

      NwStore clone = store.Clone(startLocation, null, false);

      Assert.IsNotNull(clone, $"Store {storeResRef} was null after clone.");
      Assert.IsTrue(clone.IsValid, $"Store {storeResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.AreEqual(store.Tag, clone.Tag, "Cloned store's tag did not match the original store's.");
    }

    [TearDown]
    public void CleanupTestObject()
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
