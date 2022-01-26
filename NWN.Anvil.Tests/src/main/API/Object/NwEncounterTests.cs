using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwEncounterTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "Creating a encounter with a valid ResRef creates a valid encounter.")]
    [TestCase(StandardResRef.Encounter.x2_beholder001)]
    [TestCase(StandardResRef.Encounter.x2_golem001)]
    [TestCase(StandardResRef.Encounter.nw_dragonweak)]
    [TestCase(StandardResRef.Encounter.x2_lich001)]
    [TestCase(StandardResRef.Encounter.x2_mindflay001)]
    [TestCase(StandardResRef.Encounter.nw_mummies)]
    [TestCase(StandardResRef.Encounter.x2_slaad001)]
    [TestCase(StandardResRef.Encounter.x2_undead001)]
    [TestCase(StandardResRef.Encounter.nw_undeadhigh)]
    [TestCase(StandardResRef.Encounter.x2_beholder001)]
    public void CreateEncounterIsCreated(string encounterResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwEncounter encounter = NwEncounter.Create(encounterResRef, startLocation);

      Assert.That(encounter, Is.Not.Null, $"Encounter {encounterResRef} was null after creation.");
      Assert.That(encounter.IsValid, Is.True, $"Encounter {encounterResRef} was invalid after creation.");

      createdTestObjects.Add(encounter);
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
