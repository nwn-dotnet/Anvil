using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.Services.API.Creature
{
  [TestFixture(Category = "Services.API")]
  public sealed class DamageLevelOverrideServiceTests
  {
    [Inject]
    private DamageLevelOverrideService DamageLevelOverrideService { get; init; }

    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "A created temporary resource is available as a game resource.")]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    public void SetDamageLevelOverrideChangesDamageLevel(int damageLevelIndex)
    {
      Location startLocation = NwModule.Instance.StartingLocation;

      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);
      Assert.That(creature, Is.Not.Null, "Creature was null after creation.");

      createdTestObjects.Add(creature);

      Assert.That(creature.DamageLevel, Is.EqualTo(NwGameTables.DamageLevelTable[0])); // Uninjured

      DamageLevelEntry damageLevel = NwGameTables.DamageLevelTable[damageLevelIndex];

      DamageLevelOverrideService.SetDamageLevelOverride(creature, damageLevel);

      Assert.That(DamageLevelOverrideService.GetDamageLevelOverride(creature), Is.EqualTo(damageLevel));
      Assert.That(creature.GetDamageLevelOverride(), Is.EqualTo(damageLevel));
      Assert.That(creature.DamageLevel, Is.EqualTo(damageLevel));
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
