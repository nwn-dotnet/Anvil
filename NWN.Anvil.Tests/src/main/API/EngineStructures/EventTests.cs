using NUnit.Framework;
using NWN.Core;
using Event = Anvil.API.Event;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public sealed class EventTests
  {
    [Test(Description = "Creating an event and disposing the event explicitly frees the associated memory.")]
    public void CreateAndDisposeEventFreesNativeStructure()
    {
      Event nwEvent = NWScript.EventUserDefined(0xDEAD)!;
      Assert.That(nwEvent.IsValid, Is.True, "Event was not valid after creation.");
      nwEvent.Dispose();
      Assert.That(nwEvent.IsValid, Is.False, "Event was still valid after disposing.");
    }
  }
}
