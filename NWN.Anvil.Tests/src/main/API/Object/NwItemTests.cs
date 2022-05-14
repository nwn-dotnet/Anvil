using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwItemTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

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
