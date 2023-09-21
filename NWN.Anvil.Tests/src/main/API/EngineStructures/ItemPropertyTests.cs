using Anvil.API;
using Anvil.Native;
using NUnit.Framework;
using NWN.Native.API;
using ItemProperty = Anvil.API.ItemProperty;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public sealed class ItemPropertyTests
  {
    [Test(Description = "Creating an item property and disposing the item property explicitly frees the associated memory.")]
    public void CreateAndDisposeItemPropertyFreesNativeStructure()
    {
      ItemProperty itemProperty = ItemProperty.Haste();
      Assert.That(itemProperty.IsValid, Is.True, "Item property was not valid after creation.");
      itemProperty.Dispose();
      Assert.That(itemProperty.IsValid, Is.False, "Item property was still valid after disposing.");
    }

    [Test(Description = "A soft item property reference created from a native object does not cause the original item property to be deleted.")]
    public void CreateSoftItemPropertyReferenceAndDisposeDoesNotFreeMemory()
    {
      ItemProperty itemProperty = ItemProperty.Haste();
      Assert.That(itemProperty.IsValid, Is.True, "Item property was not valid after creation.");

      CGameEffect gameEffect = itemProperty;
      Assert.That(gameEffect, Is.Not.Null, "Native Item property was not valid after implicit cast.");

      ItemProperty softReference = gameEffect.ToItemProperty(false)!;
      softReference.Dispose();
      Assert.That(softReference.IsValid, Is.True, "The soft reference disposed the memory of the original item property.");
    }
  }
}
