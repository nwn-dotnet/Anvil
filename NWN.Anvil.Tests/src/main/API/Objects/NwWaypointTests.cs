using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwWaypointTests
  {
    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Serializing and deserializing a waypoint generates valid gff data, and a new valid waypoint.")]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit)]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit002)]
    [TestCase(StandardResRef.Waypoint.x2_im_mockuploc)]
    [TestCase(StandardResRef.Waypoint.nw_wp_detect)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_home)]
    [TestCase(StandardResRef.Waypoint.nw_mapnote001)]
    [TestCase(StandardResRef.Waypoint.nw_post001)]
    [TestCase(StandardResRef.Waypoint.nw_wp_safe)]
    [TestCase(StandardResRef.Waypoint.nw_wp_shop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stealth)]
    [TestCase(StandardResRef.Waypoint.nw_wp_tavern)]
    [TestCase(StandardResRef.Waypoint.nw_waypoint001)]
    public void SerializeWaypointCreatesValidData(string waypointResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwWaypoint? waypoint = NwWaypoint.Create(waypointResRef, startLocation);

      Assert.That(waypoint, Is.Not.Null, $"Waypoint {waypointResRef} was null after creation.");
      Assert.That(waypoint!.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after creation.");

      createdTestObjects.Add(waypoint);

      byte[]? waypointData = waypoint.Serialize();

      Assert.That(waypointData, Is.Not.Null);
      Assert.That(waypointData, Has.Length.GreaterThan(0));

      NwWaypoint? waypoint2 = NwWaypoint.Deserialize(waypointData!);
      Assert.That(waypoint2, Is.Not.Null);
      Assert.That(waypoint2!.IsValid, Is.True);

      createdTestObjects.Add(waypoint2);

      Assert.That(waypoint2.Area, Is.Null);
    }

    [Test(Description = "Creating a waypoint with a valid ResRef creates a valid waypoint.")]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit)]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit002)]
    [TestCase(StandardResRef.Waypoint.x2_im_mockuploc)]
    [TestCase(StandardResRef.Waypoint.nw_wp_detect)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_home)]
    [TestCase(StandardResRef.Waypoint.nw_mapnote001)]
    [TestCase(StandardResRef.Waypoint.nw_post001)]
    [TestCase(StandardResRef.Waypoint.nw_wp_safe)]
    [TestCase(StandardResRef.Waypoint.nw_wp_shop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stealth)]
    [TestCase(StandardResRef.Waypoint.nw_wp_tavern)]
    [TestCase(StandardResRef.Waypoint.nw_waypoint001)]
    public void CreateWaypointIsCreated(string waypointResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwWaypoint? waypoint = NwWaypoint.Create(waypointResRef, startLocation);

      Assert.That(waypoint, Is.Not.Null, $"Waypoint {waypointResRef} was null after creation.");
      Assert.That(waypoint!.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after creation.");

      createdTestObjects.Add(waypoint);
    }

    [Test(Description = "Cloning a waypoint with copyLocalState = true copies expected local state information.")]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit)]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit002)]
    [TestCase(StandardResRef.Waypoint.x2_im_mockuploc)]
    [TestCase(StandardResRef.Waypoint.nw_wp_detect)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_home)]
    [TestCase(StandardResRef.Waypoint.nw_mapnote001)]
    [TestCase(StandardResRef.Waypoint.nw_post001)]
    [TestCase(StandardResRef.Waypoint.nw_wp_safe)]
    [TestCase(StandardResRef.Waypoint.nw_wp_shop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stealth)]
    [TestCase(StandardResRef.Waypoint.nw_wp_tavern)]
    [TestCase(StandardResRef.Waypoint.nw_waypoint001)]
    public void CloneWaypointWithLocalStateIsCopied(string waypointResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwWaypoint? waypoint = NwWaypoint.Create(waypointResRef, startLocation);

      Assert.That(waypoint, Is.Not.Null, $"Waypoint {waypointResRef} was null after creation.");
      Assert.That(waypoint!.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after creation.");

      createdTestObjects.Add(waypoint);

      LocalVariableInt testVar = waypoint.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwWaypoint clone = waypoint.Clone(startLocation);

      Assert.That(clone, Is.Not.Null, $"Waypoint {waypointResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.True, "Local variable did not exist on the clone with copyLocalState = true.");
      Assert.That(cloneTestVar.Value, Is.EqualTo(testVar.Value), "Local variable on the cloned waypoint did not match the value of the original waypoint.");
    }

    [Test(Description = "Cloning a waypoint with copyLocalState = false does not copy local state information.")]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit)]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit002)]
    [TestCase(StandardResRef.Waypoint.x2_im_mockuploc)]
    [TestCase(StandardResRef.Waypoint.nw_wp_detect)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_home)]
    [TestCase(StandardResRef.Waypoint.nw_mapnote001)]
    [TestCase(StandardResRef.Waypoint.nw_post001)]
    [TestCase(StandardResRef.Waypoint.nw_wp_safe)]
    [TestCase(StandardResRef.Waypoint.nw_wp_shop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stealth)]
    [TestCase(StandardResRef.Waypoint.nw_wp_tavern)]
    [TestCase(StandardResRef.Waypoint.nw_waypoint001)]
    public void CloneWaypointNoLocalStateIsNotCopied(string waypointResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwWaypoint? waypoint = NwWaypoint.Create(waypointResRef, startLocation);

      Assert.That(waypoint, Is.Not.Null, $"Waypoint {waypointResRef} was null after creation.");
      Assert.That(waypoint!.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after creation.");

      createdTestObjects.Add(waypoint);

      LocalVariableInt testVar = waypoint.GetObjectVariable<LocalVariableInt>("test");
      testVar.Value = 9999;

      NwWaypoint clone = waypoint.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Waypoint {waypointResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      LocalVariableInt cloneTestVar = clone.GetObjectVariable<LocalVariableInt>("test");

      Assert.That(cloneTestVar.HasValue, Is.False, "Local variable exists on the clone with copyLocalState = false.");
      Assert.That(cloneTestVar.Value, Is.Not.EqualTo(testVar.Value), "Local variable on the cloned waypoint matches the value of the original waypoint.");
    }

    [Test(Description = "Cloning a waypoint with a custom tag defined causes the new waypoint to gain the new tag.")]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit)]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit002)]
    [TestCase(StandardResRef.Waypoint.x2_im_mockuploc)]
    [TestCase(StandardResRef.Waypoint.nw_wp_detect)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_home)]
    [TestCase(StandardResRef.Waypoint.nw_mapnote001)]
    [TestCase(StandardResRef.Waypoint.nw_post001)]
    [TestCase(StandardResRef.Waypoint.nw_wp_safe)]
    [TestCase(StandardResRef.Waypoint.nw_wp_shop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stealth)]
    [TestCase(StandardResRef.Waypoint.nw_wp_tavern)]
    [TestCase(StandardResRef.Waypoint.nw_waypoint001)]
    public void CloneWaypointCustomTagIsApplied(string waypointResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwWaypoint? waypoint = NwWaypoint.Create(waypointResRef, startLocation);

      Assert.That(waypoint, Is.Not.Null, $"Waypoint {waypointResRef} was null after creation.");
      Assert.That(waypoint!.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after creation.");

      createdTestObjects.Add(waypoint);

      string expectedNewTag = "expectedNewTag";
      NwWaypoint clone = waypoint.Clone(startLocation, expectedNewTag, false);

      Assert.That(clone, Is.Not.Null, $"Waypoint {waypointResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(expectedNewTag), "Tag defined in clone method was not applied to the cloned waypoint.");
    }

    [Test(Description = "Cloning a waypoint with no tag defined uses the original waypoint's tag instead.")]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit)]
    [TestCase(StandardResRef.Waypoint.x2_wp_behexit002)]
    [TestCase(StandardResRef.Waypoint.x2_im_mockuploc)]
    [TestCase(StandardResRef.Waypoint.nw_wp_detect)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_home)]
    [TestCase(StandardResRef.Waypoint.nw_mapnote001)]
    [TestCase(StandardResRef.Waypoint.nw_post001)]
    [TestCase(StandardResRef.Waypoint.nw_wp_safe)]
    [TestCase(StandardResRef.Waypoint.nw_wp_shop)]
    [TestCase(StandardResRef.Waypoint.nw_wp_stealth)]
    [TestCase(StandardResRef.Waypoint.nw_wp_tavern)]
    [TestCase(StandardResRef.Waypoint.nw_waypoint001)]
    public void CloneWaypointWithoutTagOriginalTagIsCopied(string waypointResRef)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwWaypoint? waypoint = NwWaypoint.Create(waypointResRef, startLocation);

      Assert.That(waypoint, Is.Not.Null, $"Waypoint {waypointResRef} was null after creation.");
      Assert.That(waypoint!.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after creation.");

      createdTestObjects.Add(waypoint);
      waypoint.Tag = "expectedNewTag";

      NwWaypoint clone = waypoint.Clone(startLocation, null, false);

      Assert.That(clone, Is.Not.Null, $"Waypoint {waypointResRef} was null after clone.");
      Assert.That(clone.IsValid, Is.True, $"Waypoint {waypointResRef} was invalid after clone.");

      createdTestObjects.Add(clone);

      Assert.That(clone.Tag, Is.EqualTo(waypoint.Tag), "Cloned waypoint's tag did not match the original waypoint's.");
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
