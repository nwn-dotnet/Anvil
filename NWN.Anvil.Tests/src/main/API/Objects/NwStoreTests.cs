using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwStoreTests
  {
    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Serializing and deserializing a store generates valid gff data, and a new valid store.")]
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
    public void SerializeStoreCreatesValidData(string storeResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwStore? store = NwStore.Create(storeResRef, startLocation);

      Assert.That(store, Is.Not.Null, $"Store {storeResRef} was null after creation.");
      Assert.That(store!.IsValid, Is.True, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);

      byte[]? storeData = store.Serialize();

      Assert.That(storeData, Is.Not.Null);
      Assert.That(storeData, Has.Length.GreaterThan(0));

      NwStore? store2 = NwStore.Deserialize(storeData!);
      Assert.That(store2, Is.Not.Null);
      Assert.That(store2!.IsValid, Is.True);

      createdTestObjects.Add(store2);

      Assert.That(store2.Area, Is.Null);
    }

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
      NwStore? store = NwStore.Create(storeResRef, startLocation);

      Assert.That(store, Is.Not.Null, $"Store {storeResRef} was null after creation.");
      Assert.That(store!.IsValid, Is.True, $"Store {storeResRef} was invalid after creation.");

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
      NwStore? store = NwStore.Create(storeResRef, startLocation);

      Assert.That(store, Is.Not.Null, $"Store {storeResRef} was null after creation.");
      Assert.That(store!.IsValid, Is.True, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);

      LocalVariableInt testVar = store.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwStore clone = store.Clone(startLocation);

      Assert.That(clone, Is.Not.Null, $"Store {storeResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Store {storeResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.True, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.That(cloneTestVar.Value, Is.EqualTo(testVar.Value), "Local variable on the cloned store did not match the value of the original store.");
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
      NwStore? store = NwStore.Create(storeResRef, startLocation);

      Assert.That(store, Is.Not.Null, $"Store {storeResRef} was null after creation.");
      Assert.That(store!.IsValid, Is.True, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);

      LocalVariableInt testVar = store.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwStore clone = store.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Store {storeResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Store {storeResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.False, "Local variable exists on the clone with copyLocalState = false.");
      Assert.That(cloneTestVar.Value, Is.Not.EqualTo(testVar.Value), "Local variable on the cloned store matches the value of the original store.");
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
      NwStore? store = NwStore.Create(storeResRef, startLocation);

      Assert.That(store, Is.Not.Null, $"Store {storeResRef} was null after creation.");
      Assert.That(store!.IsValid, Is.True, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);

      string expectedNewTag = "expectedNewTag";
      NwStore clone = store.Clone(startLocation, expectedNewTag, false);

      Assert.That(clone, Is.Not.Null, $"Store {storeResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Store {storeResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(expectedNewTag), "Tag defined in clone method was not applied to the cloned store.");
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
      NwStore? store = NwStore.Create(storeResRef, startLocation);

      Assert.That(store, Is.Not.Null, $"Store {storeResRef} was null after creation.");
      Assert.That(store!.IsValid, Is.True, $"Store {storeResRef} was invalid after creation.");

      createdTestObjects.Add(store);
      store.Tag = "expectedNewTag";

      NwStore clone = store.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Store {storeResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Store {storeResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(store.Tag), "Cloned store's tag did not match the original store's.");
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
