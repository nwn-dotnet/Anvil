using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwSoundTests
  {
    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Serializing and deserializing a sound generates valid gff data, and a new valid sound.")]
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
    public void SerializeSoundCreatesValidData(string soundResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwSound? sound = NwSound.Create(soundResRef, startLocation);

      Assert.That(sound, Is.Not.Null, $"Sound {soundResRef} was null after creation.");
      Assert.That(sound!.IsValid, Is.True, $"Sound {soundResRef} was invalid after creation.");

      createdTestObjects.Add(sound);

      byte[]? soundData = sound.Serialize();

      Assert.That(soundData, Is.Not.Null);
      Assert.That(soundData, Has.Length.GreaterThan(0));

      NwSound? sound2 = NwSound.Deserialize(soundData!);
      Assert.That(sound2, Is.Not.Null);
      Assert.That(sound2!.IsValid, Is.True);

      createdTestObjects.Add(sound2);

      Assert.That(sound2.Area, Is.Null);
    }

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
