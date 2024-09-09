using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public abstract class ItemPropertyTest
  {
    protected abstract ItemProperty ItemProperty { get; }

    protected abstract ItemPropertyType PropertyType { get; }

    protected virtual int SubType => -1;
    protected virtual ItemPropertyCostTablesEntry? CostTable => null;
    protected virtual int CostTableValue => -1;
    protected virtual ItemPropertyParamTablesEntry? Param1Table => null;
    protected virtual int Param1TableValue => -1;
    protected virtual int UsesPerDay => -1;
    protected virtual int ChanceOfAppearing => 100;
    protected virtual int Usable => 1;
    protected virtual string CustomTag => "";

    [Test(Description = "The created item property supplies the correct parameters to the effect structure")]
    public void TestEffect()
    {
      ItemProperty itemProperty = ItemProperty;
      Assert.That(itemProperty.IsValid, Is.True);

      EffectParams<int> intParams = itemProperty.IntParams;
      EffectParams<string> stringParams = itemProperty.StringParams;

      Assert.That(intParams[0], Is.EqualTo((int)PropertyType), "Property type does not match");
      Assert.That(intParams[1], Is.EqualTo(SubType), "Property sub type does not match");
      Assert.That(intParams[2], Is.EqualTo(CostTable?.RowIndex ?? -1), "Property cost table does not match");
      Assert.That(intParams[3], Is.EqualTo(CostTableValue), "Property cost table value does not match");
      Assert.That(intParams[4], Is.EqualTo(Param1Table?.RowIndex ?? -1), "Property param1 table does not match");
      Assert.That(intParams[5], Is.EqualTo(Param1TableValue), "Property param1 table value does not match");
      Assert.That(intParams[6], Is.EqualTo(UsesPerDay), "Property uses per day does not match");
      Assert.That(intParams[7], Is.EqualTo(ChanceOfAppearing), "Property chance of appearing does not match");
      Assert.That(intParams[8], Is.EqualTo(Usable), "Property usable does not match");
      Assert.That(stringParams[0], Is.EqualTo(CustomTag), "Property custom tag does not match");
    }
  }
}
