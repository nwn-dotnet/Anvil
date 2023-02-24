using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public sealed class CassowaryTests
  {
    [Test(Description = "Creating a new cassowary and disposing the cassowary explicitly frees the associated memory.")]
    public void CreateAndDisposeCassowaryValidPropertyUpdated()
    {
      Cassowary cassowary = new Cassowary();
      Assert.That(cassowary.IsValid, Is.True, "Cassowary was not valid after creation.");
      cassowary.Dispose();
      Assert.That(cassowary.IsValid, Is.False, "Cassowary was still valid after disposing.");
    }

    [Test(Description = "A cassowary correctly finds a solution from a set of constraints.")]
    public void CreateCassowaryConstraintReturnsValidSolution()
    {
      Cassowary cassowary = new Cassowary();

      cassowary.AddConstraint("middle == (left + right) / 2");
      cassowary.AddConstraint("right == left + 10");
      cassowary.AddConstraint("right <= 100");
      cassowary.AddConstraint("left >= 0");

      Assert.That(cassowary.GetValue("left"), Is.EqualTo(90f));
      Assert.That(cassowary.GetValue("middle"), Is.EqualTo(95f));
      Assert.That(cassowary.GetValue("right"), Is.EqualTo(100f));
    }

    [Test(Description = "A cassowary correctly finds a solution from a set of constraints and a suggested value.")]
    public void CreateCassowaryConstraintWithSuggestedValueReturnsValidSolution()
    {
      Cassowary cassowary = new Cassowary();

      cassowary.AddConstraint("middle == (left + right) / 2");
      cassowary.AddConstraint("right == left + 10");
      cassowary.AddConstraint("right <= 100");
      cassowary.AddConstraint("left >= 0");

      cassowary.SuggestValue("middle", 45f);

      Assert.That(cassowary.GetValue("left"), Is.EqualTo(40f));
      Assert.That(cassowary.GetValue("middle"), Is.EqualTo(45f));
      Assert.That(cassowary.GetValue("right"), Is.EqualTo(50f));
    }
  }
}
