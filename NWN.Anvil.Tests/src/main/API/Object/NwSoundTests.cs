using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwSoundTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "Creating a sound with a valid ResRef creates a valid sound.")]
    [TestCase(StandardResRef.Sound.animalcriesday)]
    [TestCase(StandardResRef.Sound.bugnestloop)]
    [TestCase(StandardResRef.Sound.fly)]
    [TestCase(StandardResRef.Sound.cavecreatures)]
    [TestCase(StandardResRef.Sound.songbirdchirps)]
    [TestCase(StandardResRef.Sound.bellrings)]
    [TestCase(StandardResRef.Sound.cityguttersplash)]
    [TestCase(StandardResRef.Sound.firebowl)]
    [TestCase(StandardResRef.Sound.firemedium)]
    [TestCase(StandardResRef.Sound.fireplacepot)]
    public void CreateSoundIsCreated(string soundResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwSound? sound = NwSound.Create(soundResRef, startLocation);

      Assert.That(sound, Is.Not.Null, $"Sound {soundResRef} was null after creation.");
      Assert.That(sound!.IsValid, Is.True, $"Sound {soundResRef} was invalid after creation.");

      createdTestObjects.Add(sound);
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
