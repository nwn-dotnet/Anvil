using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public class NativeUtilsTests
  {
    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Creating a creature with a valid ResRef creates a valid creature.")]
    public void ExtractLocStringReturnsValidData()
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.That(creature, Is.Not.Null);
      Assert.That(creature!.IsValid, Is.True);

      createdTestObjects.Add(creature);

      string firstName = creature.OriginalFirstName;
      string lastName = creature.OriginalLastName;

      Assert.That(firstName, Is.EqualTo(string.Empty));
      Assert.That(lastName, Is.EqualTo(string.Empty));
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
