using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Variable")]
  public class LocalVariableTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "Getting a non-existing variable on an object returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    public void GetMissingLocalVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.IsNotNull(creature);
      createdTestObjects.Add(creature);

      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableBool>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableFloat>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableGuid>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableInt>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableString>(variableName));
    }

    [Test(Description = "Setting/getting a variable on an object returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    public void GetValidLocalVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.IsNotNull(creature);
      createdTestObjects.Add(creature);

      creature.GetObjectVariable<LocalVariableBool>(variableName).Value = true;
      creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName).Value = ValidEnum.TestA;
      creature.GetObjectVariable<LocalVariableFloat>(variableName).Value = 999f;
      creature.GetObjectVariable<LocalVariableGuid>(variableName).Value = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f");
      creature.GetObjectVariable<LocalVariableInt>(variableName).Value = 506;
      creature.GetObjectVariable<LocalVariableString>(variableName).Value = "test_string";

      VariableAssert(true, true, creature.GetObjectVariable<LocalVariableBool>(variableName));
      VariableAssert(true, ValidEnum.TestA, creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName));
      VariableAssert(true, 999f, creature.GetObjectVariable<LocalVariableFloat>(variableName));
      VariableAssert(true, Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"), creature.GetObjectVariable<LocalVariableGuid>(variableName));
      VariableAssert(true, 506, creature.GetObjectVariable<LocalVariableInt>(variableName));
      VariableAssert(true, "test_string", creature.GetObjectVariable<LocalVariableString>(variableName));
    }

    [Test(Description = "Setting a variable on an object and deleting it returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    public void DeleteLocalVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.IsNotNull(creature);
      createdTestObjects.Add(creature);

      creature.GetObjectVariable<LocalVariableBool>(variableName).Value = true;
      creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName).Value = ValidEnum.TestA;
      creature.GetObjectVariable<LocalVariableFloat>(variableName).Value = 999f;
      creature.GetObjectVariable<LocalVariableGuid>(variableName).Value = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f");
      creature.GetObjectVariable<LocalVariableInt>(variableName).Value = 506;
      creature.GetObjectVariable<LocalVariableString>(variableName).Value = "test_string";

      creature.GetObjectVariable<LocalVariableBool>(variableName).Delete();
      creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName).Delete();
      creature.GetObjectVariable<LocalVariableFloat>(variableName).Delete();
      creature.GetObjectVariable<LocalVariableGuid>(variableName).Delete();
      creature.GetObjectVariable<LocalVariableInt>(variableName).Delete();
      creature.GetObjectVariable<LocalVariableString>(variableName).Delete();

      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableBool>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableFloat>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableGuid>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableInt>(variableName));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableString>(variableName));
    }

    [Test(Description = "Attempting to create an object enum variable with an incorrect size throws an exception.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    public void GetInvalidEnumVariableThrowsException(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.IsNotNull(creature);
      createdTestObjects.Add(creature);

      Assert.Throws(typeof(TargetInvocationException), () =>
      {
        creature.GetObjectVariable<LocalVariableEnum<InvalidEnumA>>(variableName).Value = InvalidEnumA.TestA;
      });

      Assert.Throws(typeof(TargetInvocationException), () =>
      {
        creature.GetObjectVariable<LocalVariableEnum<InvalidEnumB>>(variableName).Value = InvalidEnumB.TestB;
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
