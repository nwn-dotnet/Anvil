using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwItemTests
  {
    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Serializing and deserializing an item generates valid gff data, and a new valid item.")]
    [TestCase(StandardResRef.Item.nw_cloth027)]
    [TestCase(StandardResRef.Item.x2_it_adaplate)]
    [TestCase(StandardResRef.Item.x0_maarcl037)]
    [TestCase(StandardResRef.Item.x0_armhe014)]
    [TestCase(StandardResRef.Item.nw_it_crewps019)]
    [TestCase(StandardResRef.Item.nw_crewphdfcl)]
    [TestCase(StandardResRef.Item.x1_it_mbook001)]
    [TestCase(StandardResRef.Item.x2_it_mbelt001)]
    [TestCase(StandardResRef.Item.nw_it_mboots002)]
    [TestCase(StandardResRef.Item.nw_it_mbracer002)]
    [TestCase(StandardResRef.Item.x0_maarcl039)]
    [TestCase(StandardResRef.Item.x1_it_mglove001)]
    [TestCase(StandardResRef.Item.x2_it_cmat_adam)]
    [TestCase(StandardResRef.Item.x2_it_dyec00)]
    [TestCase(StandardResRef.Item.x2_it_amt_feath)]
    [TestCase(StandardResRef.Item.nw_it_gem013)]
    [TestCase(StandardResRef.Item.x2_is_drose)]
    [TestCase(StandardResRef.Item.nw_it_mneck032)]
    [TestCase(StandardResRef.Item.nw_it_mring025)]
    [TestCase(StandardResRef.Item.x2_it_trap001)]
    [TestCase(StandardResRef.Item.nw_it_medkit003)]
    [TestCase(StandardResRef.Item.x0_it_mmedmisc03)]
    [TestCase(StandardResRef.Item.x0_it_mthnmisc11)]
    [TestCase(StandardResRef.Item.nw_it_mpotion003)]
    [TestCase(StandardResRef.Item.x2_it_spdvscr103)]
    [TestCase(StandardResRef.Item.nw_hen_bod3qt)]
    [TestCase(StandardResRef.Item.nw_wammar002)]
    [TestCase(StandardResRef.Item.nw_wammbo001)]
    [TestCase(StandardResRef.Item.nw_wammbu008)]
    [TestCase(StandardResRef.Item.nw_waxmgr009)]
    [TestCase(StandardResRef.Item.nw_wswmbs004)]
    [TestCase(StandardResRef.Item.nw_wswmdg004)]
    [TestCase(StandardResRef.Item.nw_wmgwn011)]
    [TestCase(StandardResRef.Item.nw_wbwmsh005)]
    [TestCase(StandardResRef.Item.x1_wmgrenade005)]
    [TestCase(StandardResRef.Item.nw_wthmsh003)]
    public void SerializeItemCreatesValidData(string itemResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? item = NwItem.Create(itemResRef, startLocation);

      Assert.That(item, Is.Not.Null, $"Item {itemResRef} was null after creation.");
      Assert.That(item!.IsValid, Is.True, $"Item {itemResRef} was invalid after creation.");

      createdTestObjects.Add(item);

      byte[]? itemData = item.Serialize();

      Assert.That(itemData, Is.Not.Null);
      Assert.That(itemData, Has.Length.GreaterThan(0));

      NwItem? item2 = NwItem.Deserialize(itemData!);
      Assert.That(item2, Is.Not.Null);
      Assert.That(item2!.IsValid, Is.True);

      createdTestObjects.Add(item2);

      Assert.That(item2.Area, Is.Null);
    }

    [Test(Description = "Creating a item with a valid ResRef creates a valid item.")]
    [TestCase(StandardResRef.Item.nw_cloth027)]
    [TestCase(StandardResRef.Item.x2_it_adaplate)]
    [TestCase(StandardResRef.Item.x0_maarcl037)]
    [TestCase(StandardResRef.Item.x0_armhe014)]
    [TestCase(StandardResRef.Item.nw_it_crewps019)]
    [TestCase(StandardResRef.Item.nw_crewphdfcl)]
    [TestCase(StandardResRef.Item.x1_it_mbook001)]
    [TestCase(StandardResRef.Item.x2_it_mbelt001)]
    [TestCase(StandardResRef.Item.nw_it_mboots002)]
    [TestCase(StandardResRef.Item.nw_it_mbracer002)]
    [TestCase(StandardResRef.Item.x0_maarcl039)]
    [TestCase(StandardResRef.Item.x1_it_mglove001)]
    [TestCase(StandardResRef.Item.x2_it_cmat_adam)]
    [TestCase(StandardResRef.Item.x2_it_dyec00)]
    [TestCase(StandardResRef.Item.x2_it_amt_feath)]
    [TestCase(StandardResRef.Item.nw_it_gem013)]
    [TestCase(StandardResRef.Item.x2_is_drose)]
    [TestCase(StandardResRef.Item.nw_it_mneck032)]
    [TestCase(StandardResRef.Item.nw_it_mring025)]
    [TestCase(StandardResRef.Item.x2_it_trap001)]
    [TestCase(StandardResRef.Item.nw_it_medkit003)]
    [TestCase(StandardResRef.Item.x0_it_mmedmisc03)]
    [TestCase(StandardResRef.Item.x0_it_mthnmisc11)]
    [TestCase(StandardResRef.Item.nw_it_mpotion003)]
    [TestCase(StandardResRef.Item.x2_it_spdvscr103)]
    [TestCase(StandardResRef.Item.nw_hen_bod3qt)]
    [TestCase(StandardResRef.Item.nw_wammar002)]
    [TestCase(StandardResRef.Item.nw_wammbo001)]
    [TestCase(StandardResRef.Item.nw_wammbu008)]
    [TestCase(StandardResRef.Item.nw_waxmgr009)]
    [TestCase(StandardResRef.Item.nw_wswmbs004)]
    [TestCase(StandardResRef.Item.nw_wswmdg004)]
    [TestCase(StandardResRef.Item.nw_wmgwn011)]
    [TestCase(StandardResRef.Item.nw_wbwmsh005)]
    [TestCase(StandardResRef.Item.x1_wmgrenade005)]
    [TestCase(StandardResRef.Item.nw_wthmsh003)]
    public void CreateItemIsCreated(string itemResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? item = NwItem.Create(itemResRef, startLocation);

      Assert.That(item, Is.Not.Null, $"Item {itemResRef} was null after creation.");
      Assert.That(item!.IsValid, Is.True, $"Item {itemResRef} was invalid after creation.");

      createdTestObjects.Add(item);
    }

    [Test(Description = "Cloning a item with copyLocalState = true copies expected local state information.")]
    [TestCase(StandardResRef.Item.nw_cloth027)]
    [TestCase(StandardResRef.Item.x2_it_adaplate)]
    [TestCase(StandardResRef.Item.x0_maarcl037)]
    [TestCase(StandardResRef.Item.x0_armhe014)]
    [TestCase(StandardResRef.Item.nw_it_crewps019)]
    [TestCase(StandardResRef.Item.nw_crewphdfcl)]
    [TestCase(StandardResRef.Item.x1_it_mbook001)]
    [TestCase(StandardResRef.Item.x2_it_mbelt001)]
    [TestCase(StandardResRef.Item.nw_it_mboots002)]
    [TestCase(StandardResRef.Item.nw_it_mbracer002)]
    [TestCase(StandardResRef.Item.x0_maarcl039)]
    [TestCase(StandardResRef.Item.x1_it_mglove001)]
    [TestCase(StandardResRef.Item.x2_it_cmat_adam)]
    [TestCase(StandardResRef.Item.x2_it_dyec00)]
    [TestCase(StandardResRef.Item.x2_it_amt_feath)]
    [TestCase(StandardResRef.Item.nw_it_gem013)]
    [TestCase(StandardResRef.Item.x2_is_drose)]
    [TestCase(StandardResRef.Item.nw_it_mneck032)]
    [TestCase(StandardResRef.Item.nw_it_mring025)]
    [TestCase(StandardResRef.Item.x2_it_trap001)]
    [TestCase(StandardResRef.Item.nw_it_medkit003)]
    [TestCase(StandardResRef.Item.x0_it_mmedmisc03)]
    [TestCase(StandardResRef.Item.x0_it_mthnmisc11)]
    [TestCase(StandardResRef.Item.nw_it_mpotion003)]
    [TestCase(StandardResRef.Item.x2_it_spdvscr103)]
    [TestCase(StandardResRef.Item.nw_hen_bod3qt)]
    [TestCase(StandardResRef.Item.nw_wammar002)]
    [TestCase(StandardResRef.Item.nw_wammbo001)]
    [TestCase(StandardResRef.Item.nw_wammbu008)]
    [TestCase(StandardResRef.Item.nw_waxmgr009)]
    [TestCase(StandardResRef.Item.nw_wswmbs004)]
    [TestCase(StandardResRef.Item.nw_wswmdg004)]
    [TestCase(StandardResRef.Item.nw_wmgwn011)]
    [TestCase(StandardResRef.Item.nw_wbwmsh005)]
    [TestCase(StandardResRef.Item.x1_wmgrenade005)]
    [TestCase(StandardResRef.Item.nw_wthmsh003)]
    public void CloneItemWithLocalStateIsCopied(string itemResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? item = NwItem.Create(itemResRef, startLocation);

      Assert.That(item, Is.Not.Null, $"Item {itemResRef} was null after creation.");
      Assert.That(item!.IsValid, Is.True, $"Item {itemResRef} was invalid after creation.");

      createdTestObjects.Add(item);

      LocalVariableInt testVar = item.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwItem clone = item.Clone(startLocation);

      Assert.That(clone, Is.Not.Null, $"Item {itemResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Item {itemResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.True, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.That(cloneTestVar.Value, Is.EqualTo(testVar.Value), "Local variable on the cloned item did not match the value of the original item.");
    }

    [Test(Description = "Cloning a item with copyLocalState = false does not copy local state information.")]
    [TestCase(StandardResRef.Item.nw_cloth027)]
    [TestCase(StandardResRef.Item.x2_it_adaplate)]
    [TestCase(StandardResRef.Item.x0_maarcl037)]
    [TestCase(StandardResRef.Item.x0_armhe014)]
    [TestCase(StandardResRef.Item.nw_it_crewps019)]
    [TestCase(StandardResRef.Item.nw_crewphdfcl)]
    [TestCase(StandardResRef.Item.x1_it_mbook001)]
    [TestCase(StandardResRef.Item.x2_it_mbelt001)]
    [TestCase(StandardResRef.Item.nw_it_mboots002)]
    [TestCase(StandardResRef.Item.nw_it_mbracer002)]
    [TestCase(StandardResRef.Item.x0_maarcl039)]
    [TestCase(StandardResRef.Item.x1_it_mglove001)]
    [TestCase(StandardResRef.Item.x2_it_cmat_adam)]
    [TestCase(StandardResRef.Item.x2_it_dyec00)]
    [TestCase(StandardResRef.Item.x2_it_amt_feath)]
    [TestCase(StandardResRef.Item.nw_it_gem013)]
    [TestCase(StandardResRef.Item.x2_is_drose)]
    [TestCase(StandardResRef.Item.nw_it_mneck032)]
    [TestCase(StandardResRef.Item.nw_it_mring025)]
    [TestCase(StandardResRef.Item.x2_it_trap001)]
    [TestCase(StandardResRef.Item.nw_it_medkit003)]
    [TestCase(StandardResRef.Item.x0_it_mmedmisc03)]
    [TestCase(StandardResRef.Item.x0_it_mthnmisc11)]
    [TestCase(StandardResRef.Item.nw_it_mpotion003)]
    [TestCase(StandardResRef.Item.x2_it_spdvscr103)]
    [TestCase(StandardResRef.Item.nw_hen_bod3qt)]
    [TestCase(StandardResRef.Item.nw_wammar002)]
    [TestCase(StandardResRef.Item.nw_wammbo001)]
    [TestCase(StandardResRef.Item.nw_wammbu008)]
    [TestCase(StandardResRef.Item.nw_waxmgr009)]
    [TestCase(StandardResRef.Item.nw_wswmbs004)]
    [TestCase(StandardResRef.Item.nw_wswmdg004)]
    [TestCase(StandardResRef.Item.nw_wmgwn011)]
    [TestCase(StandardResRef.Item.nw_wbwmsh005)]
    [TestCase(StandardResRef.Item.x1_wmgrenade005)]
    [TestCase(StandardResRef.Item.nw_wthmsh003)]
    public void CloneItemNoLocalStateIsNotCopied(string itemResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? item = NwItem.Create(itemResRef, startLocation);

      Assert.That(item, Is.Not.Null, $"Item {itemResRef} was null after creation.");
      Assert.That(item!.IsValid, Is.True, $"Item {itemResRef} was invalid after creation.");

      createdTestObjects.Add(item);

      LocalVariableInt testVar = item.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwItem clone = item.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Item {itemResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Item {itemResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.False, "Local variable exists on the clone with copyLocalState = false.");
      Assert.That(cloneTestVar.Value, Is.Not.EqualTo(testVar.Value), "Local variable on the cloned item matches the value of the original item.");
    }

    [Test(Description = "Cloning a item with a custom tag defined causes the new item to gain the new tag.")]
    [TestCase(StandardResRef.Item.nw_cloth027)]
    [TestCase(StandardResRef.Item.x2_it_adaplate)]
    [TestCase(StandardResRef.Item.x0_maarcl037)]
    [TestCase(StandardResRef.Item.x0_armhe014)]
    [TestCase(StandardResRef.Item.nw_it_crewps019)]
    [TestCase(StandardResRef.Item.nw_crewphdfcl)]
    [TestCase(StandardResRef.Item.x1_it_mbook001)]
    [TestCase(StandardResRef.Item.x2_it_mbelt001)]
    [TestCase(StandardResRef.Item.nw_it_mboots002)]
    [TestCase(StandardResRef.Item.nw_it_mbracer002)]
    [TestCase(StandardResRef.Item.x0_maarcl039)]
    [TestCase(StandardResRef.Item.x1_it_mglove001)]
    [TestCase(StandardResRef.Item.x2_it_cmat_adam)]
    [TestCase(StandardResRef.Item.x2_it_dyec00)]
    [TestCase(StandardResRef.Item.x2_it_amt_feath)]
    [TestCase(StandardResRef.Item.nw_it_gem013)]
    [TestCase(StandardResRef.Item.x2_is_drose)]
    [TestCase(StandardResRef.Item.nw_it_mneck032)]
    [TestCase(StandardResRef.Item.nw_it_mring025)]
    [TestCase(StandardResRef.Item.x2_it_trap001)]
    [TestCase(StandardResRef.Item.nw_it_medkit003)]
    [TestCase(StandardResRef.Item.x0_it_mmedmisc03)]
    [TestCase(StandardResRef.Item.x0_it_mthnmisc11)]
    [TestCase(StandardResRef.Item.nw_it_mpotion003)]
    [TestCase(StandardResRef.Item.x2_it_spdvscr103)]
    [TestCase(StandardResRef.Item.nw_hen_bod3qt)]
    [TestCase(StandardResRef.Item.nw_wammar002)]
    [TestCase(StandardResRef.Item.nw_wammbo001)]
    [TestCase(StandardResRef.Item.nw_wammbu008)]
    [TestCase(StandardResRef.Item.nw_waxmgr009)]
    [TestCase(StandardResRef.Item.nw_wswmbs004)]
    [TestCase(StandardResRef.Item.nw_wswmdg004)]
    [TestCase(StandardResRef.Item.nw_wmgwn011)]
    [TestCase(StandardResRef.Item.nw_wbwmsh005)]
    [TestCase(StandardResRef.Item.x1_wmgrenade005)]
    [TestCase(StandardResRef.Item.nw_wthmsh003)]
    public void CloneItemCustomTagIsApplied(string itemResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? item = NwItem.Create(itemResRef, startLocation);

      Assert.That(item, Is.Not.Null, $"Item {itemResRef} was null after creation.");
      Assert.That(item!.IsValid, Is.True, $"Item {itemResRef} was invalid after creation.");

      createdTestObjects.Add(item);

      string expectedNewTag = "expectedNewTag";
      NwItem clone = item.Clone(startLocation, expectedNewTag, false);

      Assert.That(clone, Is.Not.Null, $"Item {itemResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Item {itemResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(expectedNewTag), "Tag defined in clone method was not applied to the cloned item.");
    }

    [Test(Description = "Cloning a item with no tag defined uses the original item's tag instead.")]
    [TestCase(StandardResRef.Item.nw_cloth027)]
    [TestCase(StandardResRef.Item.x2_it_adaplate)]
    [TestCase(StandardResRef.Item.x0_maarcl037)]
    [TestCase(StandardResRef.Item.x0_armhe014)]
    [TestCase(StandardResRef.Item.nw_it_crewps019)]
    [TestCase(StandardResRef.Item.nw_crewphdfcl)]
    [TestCase(StandardResRef.Item.x1_it_mbook001)]
    [TestCase(StandardResRef.Item.x2_it_mbelt001)]
    [TestCase(StandardResRef.Item.nw_it_mboots002)]
    [TestCase(StandardResRef.Item.nw_it_mbracer002)]
    [TestCase(StandardResRef.Item.x0_maarcl039)]
    [TestCase(StandardResRef.Item.x1_it_mglove001)]
    [TestCase(StandardResRef.Item.x2_it_cmat_adam)]
    [TestCase(StandardResRef.Item.x2_it_dyec00)]
    [TestCase(StandardResRef.Item.x2_it_amt_feath)]
    [TestCase(StandardResRef.Item.nw_it_gem013)]
    [TestCase(StandardResRef.Item.x2_is_drose)]
    [TestCase(StandardResRef.Item.nw_it_mneck032)]
    [TestCase(StandardResRef.Item.nw_it_mring025)]
    [TestCase(StandardResRef.Item.x2_it_trap001)]
    [TestCase(StandardResRef.Item.nw_it_medkit003)]
    [TestCase(StandardResRef.Item.x0_it_mmedmisc03)]
    [TestCase(StandardResRef.Item.x0_it_mthnmisc11)]
    [TestCase(StandardResRef.Item.nw_it_mpotion003)]
    [TestCase(StandardResRef.Item.x2_it_spdvscr103)]
    [TestCase(StandardResRef.Item.nw_hen_bod3qt)]
    [TestCase(StandardResRef.Item.nw_wammar002)]
    [TestCase(StandardResRef.Item.nw_wammbo001)]
    [TestCase(StandardResRef.Item.nw_wammbu008)]
    [TestCase(StandardResRef.Item.nw_waxmgr009)]
    [TestCase(StandardResRef.Item.nw_wswmbs004)]
    [TestCase(StandardResRef.Item.nw_wswmdg004)]
    [TestCase(StandardResRef.Item.nw_wmgwn011)]
    [TestCase(StandardResRef.Item.nw_wbwmsh005)]
    [TestCase(StandardResRef.Item.x1_wmgrenade005)]
    [TestCase(StandardResRef.Item.nw_wthmsh003)]
    public void CloneItemWithoutTagOriginalTagIsCopied(string itemResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? item = NwItem.Create(itemResRef, startLocation);

      Assert.That(item, Is.Not.Null, $"Item {itemResRef} was null after creation.");
      Assert.That(item!.IsValid, Is.True, $"Item {itemResRef} was invalid after creation.");

      createdTestObjects.Add(item);
      item.Tag = "expectedNewTag";

      NwItem clone = item.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Item {itemResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Item {itemResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(item.Tag), "Cloned item's tag did not match the original item's.");
    }

    [Test(Description = "Serializing an item's appearance preserves the item appearance data.")]
    [TestCase(StandardResRef.Item.nw_cloth027)]
    [TestCase(StandardResRef.Item.x2_it_adaplate)]
    [TestCase(StandardResRef.Item.x0_maarcl037)]
    [TestCase(StandardResRef.Item.x0_armhe014)]
    [TestCase(StandardResRef.Item.nw_it_crewps019)]
    [TestCase(StandardResRef.Item.nw_crewphdfcl)]
    [TestCase(StandardResRef.Item.x1_it_mbook001)]
    [TestCase(StandardResRef.Item.x2_it_mbelt001)]
    [TestCase(StandardResRef.Item.nw_it_mboots002)]
    [TestCase(StandardResRef.Item.nw_it_mbracer002)]
    [TestCase(StandardResRef.Item.x0_maarcl039)]
    [TestCase(StandardResRef.Item.x1_it_mglove001)]
    [TestCase(StandardResRef.Item.x2_it_cmat_adam)]
    [TestCase(StandardResRef.Item.x2_it_dyec00)]
    [TestCase(StandardResRef.Item.x2_it_amt_feath)]
    [TestCase(StandardResRef.Item.nw_it_gem013)]
    [TestCase(StandardResRef.Item.x2_is_drose)]
    [TestCase(StandardResRef.Item.nw_it_mneck032)]
    [TestCase(StandardResRef.Item.nw_it_mring025)]
    [TestCase(StandardResRef.Item.x2_it_trap001)]
    [TestCase(StandardResRef.Item.nw_it_medkit003)]
    [TestCase(StandardResRef.Item.x0_it_mmedmisc03)]
    [TestCase(StandardResRef.Item.x0_it_mthnmisc11)]
    [TestCase(StandardResRef.Item.nw_it_mpotion003)]
    [TestCase(StandardResRef.Item.x2_it_spdvscr103)]
    [TestCase(StandardResRef.Item.nw_hen_bod3qt)]
    [TestCase(StandardResRef.Item.nw_wammar002)]
    [TestCase(StandardResRef.Item.nw_wammbo001)]
    [TestCase(StandardResRef.Item.nw_wammbu008)]
    [TestCase(StandardResRef.Item.nw_waxmgr009)]
    [TestCase(StandardResRef.Item.nw_wswmbs004)]
    [TestCase(StandardResRef.Item.nw_wswmdg004)]
    [TestCase(StandardResRef.Item.nw_wmgwn011)]
    [TestCase(StandardResRef.Item.nw_wbwmsh005)]
    [TestCase(StandardResRef.Item.x1_wmgrenade005)]
    [TestCase(StandardResRef.Item.nw_wthmsh003)]
    public void SerializeItemAppearancePreservesAppearance(string itemResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? item = NwItem.Create(itemResRef, startLocation);

      Assert.That(item, Is.Not.Null, $"Item {itemResRef} was null after creation.");
      Assert.That(item!.IsValid, Is.True, $"Item {itemResRef} was invalid after creation.");

      createdTestObjects.Add(item);

      string appearance = item.Appearance.Serialize();

      Assert.That(appearance.Length == 328, "Serialized length does not match expected value of 328.");

      ushort model = item.Appearance.GetSimpleModel();

      item.Appearance.Deserialize(appearance);

      Assert.That(item.Appearance.GetSimpleModel(), Is.EqualTo(model));
    }

    [Test(Description = "An item stored in an item container returns the correct possessing object.")]
    public void ItemInContainerOnGroundReturnsCorrectPossessor()
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? container = NwItem.Create(StandardResRef.Item.nw_it_contain006, startLocation);
      NwItem? item = NwItem.Create(StandardResRef.Item.nw_it_gem013, startLocation);

      Assert.That(container, Is.Not.Null, "Item nw_it_contain006 was null after creation.");
      Assert.That(container!.IsValid, Is.True, "Item nw_it_contain006 was invalid after creation.");

      createdTestObjects.Add(container);

      Assert.That(item, Is.Not.Null, "Item nw_it_gem013 was null after creation.");
      Assert.That(item!.IsValid, Is.True, "Item nw_it_gem013 was invalid after creation.");

      createdTestObjects.Add(item);

      container.AcquireItem(item);

      Assert.That(item.Possessor, Is.EqualTo(container), "Item possessor does not match expected container.");
      Assert.That(item.RootPossessor, Is.Null, "Root possessor is not null.");
    }

    [Test(Description = "An item stored in an item container on a creature returns the correct possessing object.")]
    public void ItemInContainerOnCreatureReturnsCorrectPossessor()
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? container = NwItem.Create(StandardResRef.Item.nw_it_contain006, startLocation);
      NwItem? item = NwItem.Create(StandardResRef.Item.nw_it_gem013, startLocation);
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.That(container, Is.Not.Null, "Item nw_it_contain006 was null after creation.");
      Assert.That(container!.IsValid, Is.True, "Item nw_it_contain006 was invalid after creation.");

      createdTestObjects.Add(container);

      Assert.That(item, Is.Not.Null, "Item nw_it_gem013 was null after creation.");
      Assert.That(item!.IsValid, Is.True, "Item nw_it_gem013 was invalid after creation.");

      createdTestObjects.Add(item);

      Assert.That(creature, Is.Not.Null, "Creature nw_bandit001 was null after creation.");
      Assert.That(creature!.IsValid, Is.True, "Creature nw_bandit001 was invalid after creation.");

      createdTestObjects.Add(creature);

      container.AcquireItem(item);
      creature.AcquireItem(container);

      Assert.That(item.Possessor, Is.EqualTo(container), "Item possessor does not match expected container.");
      Assert.That(item.RootPossessor, Is.EqualTo(creature), "Root possessor is not the expected creature.");
    }

    [Test(Description = "An item stored in an item container on a placeable returns the correct possessing object.")]
    public void ItemInContainerOnPlaceableReturnsCorrectPossessor()
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwItem? container = NwItem.Create(StandardResRef.Item.nw_it_contain006, startLocation);
      NwItem? item = NwItem.Create(StandardResRef.Item.nw_it_gem013, startLocation);
      NwPlaceable? placeable = NwPlaceable.Create(StandardResRef.Placeable.plc_chest1, startLocation);

      Assert.That(container, Is.Not.Null, "Item nw_it_contain006 was null after creation.");
      Assert.That(container!.IsValid, Is.True, "Item nw_it_contain006 was invalid after creation.");

      createdTestObjects.Add(container);

      Assert.That(item, Is.Not.Null, "Item nw_it_gem013 was null after creation.");
      Assert.That(item!.IsValid, Is.True, "Item nw_it_gem013 was invalid after creation.");

      createdTestObjects.Add(item);

      Assert.That(placeable, Is.Not.Null, "Placeable plc_chest1 was null after creation.");
      Assert.That(placeable!.IsValid, Is.True, "Placeable plc_chest1 was invalid after creation.");

      createdTestObjects.Add(placeable);
      placeable.HasInventory = true;

      container.AcquireItem(item);
      placeable.AcquireItem(container);

      Assert.That(item.Possessor, Is.EqualTo(container), "Item possessor does not match expected container.");
      Assert.That(item.RootPossessor, Is.EqualTo(placeable), "Root possessor is not the expected placeable.");
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
