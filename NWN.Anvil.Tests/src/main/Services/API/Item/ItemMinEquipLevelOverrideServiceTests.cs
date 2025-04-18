using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Anvil.Services.Item;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.Services.API.Item
{
  [TestFixture]
  public class ItemMinEquipLevelOverrideServiceTests
  {
    [Inject]
    private static ItemMinEquipLevelOverrideService ItemMinEquipLevelOverrideService { get; set; } = null!;

    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Setting an item minimum level overrides for a given item.")]
    [TestCase(StandardResRef.Item.nw_ashmlw009, 1, 0)]
    [TestCase(StandardResRef.Item.x2_wdrowls003, 15, 5)]
    [TestCase(StandardResRef.Item.x2_it_mcloak007, 19, 30)]
    public void SetItemMinEquipLevelOverrideChangesMinEquipLevel(string itemResRef, int standardMinLevel, byte overrideMinLevel)
    {
      Location startLocation = NwModule.Instance.StartingLocation;

      NwItem? item = NwItem.Create(itemResRef, startLocation);
      Assert.That(item, Is.Not.Null, "Item was null after creation.");

      createdTestObjects.Add(item!);

      Assert.That(item!.MinEquipLevel, Is.EqualTo(standardMinLevel), "Item has expected base min equip level.");

      ItemMinEquipLevelOverrideService.SetMinEquipLevelOverride(item, overrideMinLevel);

      Assert.That(ItemMinEquipLevelOverrideService.GetMinEquipLevelOverride(item), Is.EqualTo(overrideMinLevel));
      Assert.That(item.GetMinEquipLevelOverride(), Is.EqualTo(overrideMinLevel));
      Assert.That(item.MinEquipLevel, Is.EqualTo(overrideMinLevel));
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
