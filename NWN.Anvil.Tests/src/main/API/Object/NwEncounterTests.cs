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

    [Test(Description = "Serializing and deserializing an encounter generates valid gff data, and a new valid encounter.")]
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
    public void SerializeEncounterCreatesValidData(string encounterResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwEncounter? encounter = NwEncounter.Create(encounterResRef, startLocation);

      Assert.That(encounter, Is.Not.Null, $"Encounter {encounterResRef} was null after creation.");
      Assert.That(encounter!.IsValid, Is.True, $"Encounter {encounterResRef} was invalid after creation.");

      createdTestObjects.Add(encounter);

      byte[]? encounterData = encounter.Serialize();

      Assert.That(encounterData, Is.Not.Null);
      Assert.That(encounterData, Has.Length.GreaterThan(0));

      NwEncounter? encounter2 = NwEncounter.Deserialize(encounterData!);
      Assert.That(encounter2, Is.Not.Null);
      Assert.That(encounter2!.IsValid, Is.True);

      createdTestObjects.Add(encounter2);

      Assert.That(encounter2.Area, Is.Null);
    }

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
      NwEncounter? encounter = NwEncounter.Create(encounterResRef, startLocation);

      Assert.That(encounter, Is.Not.Null, $"Encounter {encounterResRef} was null after creation.");
      Assert.That(encounter!.IsValid, Is.True, $"Encounter {encounterResRef} was invalid after creation.");

      createdTestObjects.Add(encounter);
    }

    [Test(Description = "Creating a encounter and destroying it destroys the encounter.")]
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
    public void DestroyEncounterIsDestroyed(string encounterResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwEncounter? encounter = NwEncounter.Create(encounterResRef, startLocation);

      Assert.That(encounter, Is.Not.Null, $"Encounter {encounterResRef} was null after creation.");
      Assert.That(encounter!.IsValid, Is.True, $"Encounter {encounterResRef} was invalid after creation.");

      encounter.Destroy();
      Assert.That(encounter.IsValid, Is.False, $"Encounter {encounterResRef} was still valid after deletion.");
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
