using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API.Variable
{
  [TestFixture(Category = "API.Variable")]
  public class ObjectStorageVariableTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "Getting a non-existing variable on an object returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    [TestCase("\0")]
    [TestCase("\0aaa")]
    public void GetMissingPersistentVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.IsNotNull(creature);
      createdTestObjects.Add(creature);

      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableBool>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableFloat>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableGuid>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableInt>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableString>(variableName));
    }

    [Test(Description = "Setting/getting a variable on an object returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    [TestCase("\0")]
    [TestCase("\0aaa")]
    public void GetValidPersistentVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.IsNotNull(creature);
      createdTestObjects.Add(creature);

      creature.GetObjectVariable<PersistentVariableBool>(variableName).Value = true;
      creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName).Value = ValidEnum.TestA;
      creature.GetObjectVariable<PersistentVariableFloat>(variableName).Value = 999f;
      creature.GetObjectVariable<PersistentVariableGuid>(variableName).Value = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f");
      creature.GetObjectVariable<PersistentVariableInt>(variableName).Value = 506;
      creature.GetObjectVariable<PersistentVariableString>(variableName).Value = "test_string";

      VariableAssert(true, true, creature.GetObjectVariable<PersistentVariableBool>(variableName));
      VariableAssert(true, ValidEnum.TestA, creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName));
      VariableAssert(true, 999f, creature.GetObjectVariable<PersistentVariableFloat>(variableName));
      VariableAssert(true, Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"), creature.GetObjectVariable<PersistentVariableGuid>(variableName));
      VariableAssert(true, 506, creature.GetObjectVariable<PersistentVariableInt>(variableName));
      VariableAssert(true, "test_string", creature.GetObjectVariable<PersistentVariableString>(variableName));
    }

    [Test(Description = "Attempting to create an object enum variable with an incorrect size throws an exception.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    [TestCase("\0")]
    [TestCase("\0aaa")]
    public void GetInvalidEnumVariableThrowsException(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.IsNotNull(creature);
      createdTestObjects.Add(creature);

      Assert.Throws(typeof(ArgumentOutOfRangeException), () =>
      {
        creature.GetObjectVariable<PersistentVariableEnum<InvalidEnumA>>(variableName).Value = InvalidEnumA.TestA;
      });

      Assert.Throws(typeof(ArgumentOutOfRangeException), () =>
      {
        creature.GetObjectVariable<PersistentVariableEnum<InvalidEnumB>>(variableName).Value = InvalidEnumB.TestB;
      });
    }

    private void VariableAssert<T>(bool expectHasValue, T expectedValue, ObjectVariable<T> variable)
    {
      Assert.IsNotNull(variable, "Created variable was null.");

      if (expectHasValue)
      {
        Assert.IsTrue(variable.HasValue, "Expected variable to have value, but HasValue returned false.");
        Assert.IsFalse(variable.HasNothing, "Expected variable to have value, but HasNothing returned true.");
      }
      else
      {
        Assert.IsTrue(variable.HasNothing, "Expected variable to have no value, but HasNothing returned false.");
        Assert.IsFalse(variable.HasValue, "Expected variable to have no value, but HasValue returned true.");
      }

      Assert.AreEqual(expectedValue, variable.Value);
    }

    private enum ValidEnum
    {
      Default = 0,
      TestA = 1,
      TestB = 2,
    }

    private enum InvalidEnumA : long
    {
      Default = 0,
      TestA = 1,
      TestB = 2,
    }

    private enum InvalidEnumB : byte
    {
      Default = 0,
      TestA = 1,
      TestB = 2,
    }

    [TearDown]
    public void CleanupTestObject()
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
