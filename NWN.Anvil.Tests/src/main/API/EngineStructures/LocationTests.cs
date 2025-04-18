using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public class LocationTests
  {
    [Test(Description = "Creating a location and disposing the location explicitly frees the associated memory.")]
    public void CreateAndDisposeLocationFreesNativeStructure()
    {
      Location location = NwModule.Instance.StartingLocation;
      Assert.That(location.IsValid, Is.True, "Location was not valid after creation.");
      location.Dispose();
      Assert.That(location.IsValid, Is.False, "Location was still valid after disposing.");
    }
  }
}
