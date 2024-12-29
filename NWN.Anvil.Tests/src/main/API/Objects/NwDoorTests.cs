using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwDoorTests
  {
    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Serializing and deserializing a door generates valid gff data, and a new valid door.")]
    [TestCase(StandardResRef.Door.nw_door_grate)]
    [TestCase(StandardResRef.Door.nw_door_metal)]
    [TestCase(StandardResRef.Door.nw_door_rusted)]
    [TestCase(StandardResRef.Door.x2_doorhard1)]
    [TestCase(StandardResRef.Door.x3_door_met001)]
    [TestCase(StandardResRef.Door.nw_door_evlstone)]
    [TestCase(StandardResRef.Door.nw_door_jeweled)]
    [TestCase(StandardResRef.Door.nw_door_stone)]
    [TestCase(StandardResRef.Door.x2_doormed1)]
    [TestCase(StandardResRef.Door.x3_door_stn001)]
    [TestCase(StandardResRef.Door.nw_door_fancy)]
    [TestCase(StandardResRef.Door.nw_door_normal)]
    [TestCase(StandardResRef.Door.nw_door_strong)]
    [TestCase(StandardResRef.Door.nw_door_weak)]
    [TestCase(StandardResRef.Door.x2_dooreasy1)]
    [TestCase(StandardResRef.Door.x3_door_wood001)]
    [TestCase(StandardResRef.Door.x3_door_oth001)]
    [TestCase(StandardResRef.Door.tm_dr_elven1)]
    public void SerializeDoorCreatesValidData(string doorResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwDoor? door = NwDoor.Create(doorResRef, startLocation);

      Assert.That(door, Is.Not.Null, $"Door {doorResRef} was null after creation.");
      Assert.That(door!.IsValid, Is.True, $"Door {doorResRef} was invalid after creation.");

      createdTestObjects.Add(door);

      byte[]? doorData = door.Serialize();

      Assert.That(doorData, Is.Not.Null);
      Assert.That(doorData, Has.Length.GreaterThan(0));

      NwDoor? door2 = NwDoor.Deserialize(doorData!);
      Assert.That(door2, Is.Not.Null);
      Assert.That(door2!.IsValid, Is.True);

      createdTestObjects.Add(door2);

      Assert.That(door2.Area, Is.Null);
    }

    [Test(Description = "Creating a door with a valid ResRef creates a valid door.")]
    [TestCase(StandardResRef.Door.nw_door_grate)]
    [TestCase(StandardResRef.Door.nw_door_metal)]
    [TestCase(StandardResRef.Door.nw_door_rusted)]
    [TestCase(StandardResRef.Door.x2_doorhard1)]
    [TestCase(StandardResRef.Door.x3_door_met001)]
    [TestCase(StandardResRef.Door.nw_door_evlstone)]
    [TestCase(StandardResRef.Door.nw_door_jeweled)]
    [TestCase(StandardResRef.Door.nw_door_stone)]
    [TestCase(StandardResRef.Door.x2_doormed1)]
    [TestCase(StandardResRef.Door.x3_door_stn001)]
    [TestCase(StandardResRef.Door.nw_door_fancy)]
    [TestCase(StandardResRef.Door.nw_door_normal)]
    [TestCase(StandardResRef.Door.nw_door_strong)]
    [TestCase(StandardResRef.Door.nw_door_weak)]
    [TestCase(StandardResRef.Door.x2_dooreasy1)]
    [TestCase(StandardResRef.Door.x3_door_wood001)]
    [TestCase(StandardResRef.Door.x3_door_oth001)]
    [TestCase(StandardResRef.Door.tm_dr_elven1)]
    public void CreateDoorIsCreated(string doorResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwDoor? door = NwDoor.Create(doorResRef, startLocation);

      Assert.That(door, Is.Not.Null, $"Door {doorResRef} was null after creation.");
      Assert.That(door!.IsValid, Is.True, $"Door {doorResRef} was invalid after creation.");

      createdTestObjects.Add(door);
    }

    [Test(Description = "Cloning a door with copyLocalState = true copies expected local state information.")]
    [TestCase(StandardResRef.Door.nw_door_grate)]
    [TestCase(StandardResRef.Door.nw_door_metal)]
    [TestCase(StandardResRef.Door.nw_door_rusted)]
    [TestCase(StandardResRef.Door.x2_doorhard1)]
    [TestCase(StandardResRef.Door.x3_door_met001)]
    [TestCase(StandardResRef.Door.nw_door_evlstone)]
    [TestCase(StandardResRef.Door.nw_door_jeweled)]
    [TestCase(StandardResRef.Door.nw_door_stone)]
    [TestCase(StandardResRef.Door.x2_doormed1)]
    [TestCase(StandardResRef.Door.x3_door_stn001)]
    [TestCase(StandardResRef.Door.nw_door_fancy)]
    [TestCase(StandardResRef.Door.nw_door_normal)]
    [TestCase(StandardResRef.Door.nw_door_strong)]
    [TestCase(StandardResRef.Door.nw_door_weak)]
    [TestCase(StandardResRef.Door.x2_dooreasy1)]
    [TestCase(StandardResRef.Door.x3_door_wood001)]
    [TestCase(StandardResRef.Door.x3_door_oth001)]
    [TestCase(StandardResRef.Door.tm_dr_elven1)]
    public void CloneDoorWithLocalStateIsCopied(string doorResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwDoor? door = NwDoor.Create(doorResRef, startLocation);

      Assert.That(door, Is.Not.Null, $"Door {doorResRef} was null after creation.");
      Assert.That(door!.IsValid, Is.True, $"Door {doorResRef} was invalid after creation.");

      createdTestObjects.Add(door);

      LocalVariableInt testVar = door.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwDoor clone = door.Clone(startLocation);

      Assert.That(clone, Is.Not.Null, $"Door {doorResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Door {doorResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.True, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.That(cloneTestVar.Value, Is.EqualTo(testVar.Value), "Local variable on the cloned door did not match the value of the original door.");
    }

    [Test(Description = "Cloning a door with copyLocalState = false does not copy local state information.")]
    [TestCase(StandardResRef.Door.nw_door_grate)]
    [TestCase(StandardResRef.Door.nw_door_metal)]
    [TestCase(StandardResRef.Door.nw_door_rusted)]
    [TestCase(StandardResRef.Door.x2_doorhard1)]
    [TestCase(StandardResRef.Door.x3_door_met001)]
    [TestCase(StandardResRef.Door.nw_door_evlstone)]
    [TestCase(StandardResRef.Door.nw_door_jeweled)]
    [TestCase(StandardResRef.Door.nw_door_stone)]
    [TestCase(StandardResRef.Door.x2_doormed1)]
    [TestCase(StandardResRef.Door.x3_door_stn001)]
    [TestCase(StandardResRef.Door.nw_door_fancy)]
    [TestCase(StandardResRef.Door.nw_door_normal)]
    [TestCase(StandardResRef.Door.nw_door_strong)]
    [TestCase(StandardResRef.Door.nw_door_weak)]
    [TestCase(StandardResRef.Door.x2_dooreasy1)]
    [TestCase(StandardResRef.Door.x3_door_wood001)]
    [TestCase(StandardResRef.Door.x3_door_oth001)]
    [TestCase(StandardResRef.Door.tm_dr_elven1)]
    public void CloneDoorNoLocalStateIsNotCopied(string doorResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwDoor? door = NwDoor.Create(doorResRef, startLocation);

      Assert.That(door, Is.Not.Null, $"Door {doorResRef} was null after creation.");
      Assert.That(door!.IsValid, Is.True, $"Door {doorResRef} was invalid after creation.");

      createdTestObjects.Add(door);

      LocalVariableInt testVar = door.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwDoor clone = door.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Door {doorResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Door {doorResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.False, "Local variable exists on the clone with copyLocalState = false.");
      Assert.That(cloneTestVar.Value, Is.Not.EqualTo(testVar.Value), "Local variable on the cloned door matches the value of the original door.");
    }

    [Test(Description = "Cloning a door with a custom tag defined causes the new door to gain the new tag.")]
    [TestCase(StandardResRef.Door.nw_door_grate)]
    [TestCase(StandardResRef.Door.nw_door_metal)]
    [TestCase(StandardResRef.Door.nw_door_rusted)]
    [TestCase(StandardResRef.Door.x2_doorhard1)]
    [TestCase(StandardResRef.Door.x3_door_met001)]
    [TestCase(StandardResRef.Door.nw_door_evlstone)]
    [TestCase(StandardResRef.Door.nw_door_jeweled)]
    [TestCase(StandardResRef.Door.nw_door_stone)]
    [TestCase(StandardResRef.Door.x2_doormed1)]
    [TestCase(StandardResRef.Door.x3_door_stn001)]
    [TestCase(StandardResRef.Door.nw_door_fancy)]
    [TestCase(StandardResRef.Door.nw_door_normal)]
    [TestCase(StandardResRef.Door.nw_door_strong)]
    [TestCase(StandardResRef.Door.nw_door_weak)]
    [TestCase(StandardResRef.Door.x2_dooreasy1)]
    [TestCase(StandardResRef.Door.x3_door_wood001)]
    [TestCase(StandardResRef.Door.x3_door_oth001)]
    [TestCase(StandardResRef.Door.tm_dr_elven1)]
    public void CloneDoorCustomTagIsApplied(string doorResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwDoor? door = NwDoor.Create(doorResRef, startLocation);

      Assert.That(door, Is.Not.Null, $"Door {doorResRef} was null after creation.");
      Assert.That(door!.IsValid, Is.True, $"Door {doorResRef} was invalid after creation.");

      createdTestObjects.Add(door);

      const string expectedNewTag = "expectedNewTag";
      NwDoor clone = door.Clone(startLocation, expectedNewTag, false);

      Assert.That(clone, Is.Not.Null, $"Door {doorResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Door {doorResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(expectedNewTag), "Tag defined in clone method was not applied to the cloned door.");
    }

    [Test(Description = "Cloning a door with no tag defined uses the original door's tag instead.")]
    [TestCase(StandardResRef.Door.nw_door_grate)]
    [TestCase(StandardResRef.Door.nw_door_metal)]
    [TestCase(StandardResRef.Door.nw_door_rusted)]
    [TestCase(StandardResRef.Door.x2_doorhard1)]
    [TestCase(StandardResRef.Door.x3_door_met001)]
    [TestCase(StandardResRef.Door.nw_door_evlstone)]
    [TestCase(StandardResRef.Door.nw_door_jeweled)]
    [TestCase(StandardResRef.Door.nw_door_stone)]
    [TestCase(StandardResRef.Door.x2_doormed1)]
    [TestCase(StandardResRef.Door.x3_door_stn001)]
    [TestCase(StandardResRef.Door.nw_door_fancy)]
    [TestCase(StandardResRef.Door.nw_door_normal)]
    [TestCase(StandardResRef.Door.nw_door_strong)]
    [TestCase(StandardResRef.Door.nw_door_weak)]
    [TestCase(StandardResRef.Door.x2_dooreasy1)]
    [TestCase(StandardResRef.Door.x3_door_wood001)]
    [TestCase(StandardResRef.Door.x3_door_oth001)]
    [TestCase(StandardResRef.Door.tm_dr_elven1)]
    public void CloneDoorWithoutTagOriginalTagIsCopied(string doorResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwDoor? door = NwDoor.Create(doorResRef, startLocation);

      Assert.That(door, Is.Not.Null, $"Door {doorResRef} was null after creation.");
      Assert.That(door!.IsValid, Is.True, $"Door {doorResRef} was invalid after creation.");

      createdTestObjects.Add(door);
      door.Tag = "expectedNewTag";

      NwDoor clone = door.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Door {doorResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Door {doorResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(door.Tag), "Cloned door's tag did not match the original door's.");
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
