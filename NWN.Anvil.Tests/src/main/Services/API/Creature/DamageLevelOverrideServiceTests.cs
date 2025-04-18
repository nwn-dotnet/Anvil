using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.Services.API.Creature
{
  [TestFixture]
  public sealed class DamageLevelOverrideServiceTests
  {
    [Inject]
    private static DamageLevelOverrideService DamageLevelOverrideService { get; set; } = null!;

    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "A created temporary resource is available as a game resource.")]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    public void SetDamageLevelOverrideChangesDamageLevel(int damageLevelIndex)
    {
      Location startLocation = NwModule.Instance.StartingLocation;

      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);
      Assert.That(creature, Is.Not.Null, "Creature was null after creation.");

      createdTestObjects.Add(creature!);

      Assert.That(creature!.DamageLevel.RowIndex, Is.EqualTo(NwGameTables.DamageLevelTable[0].RowIndex)); // Uninjured

      DamageLevelEntry damageLevel = NwGameTables.DamageLevelTable[damageLevelIndex];

      DamageLevelOverrideService.SetDamageLevelOverride(creature, damageLevel);

      Assert.That(DamageLevelOverrideService.GetDamageLevelOverride(creature)?.RowIndex, Is.EqualTo(damageLevel.RowIndex));
      Assert.That(creature.GetDamageLevelOverride()?.RowIndex, Is.EqualTo(damageLevel.RowIndex));
      Assert.That(creature.DamageLevel.RowIndex, Is.EqualTo(damageLevel.RowIndex));
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
