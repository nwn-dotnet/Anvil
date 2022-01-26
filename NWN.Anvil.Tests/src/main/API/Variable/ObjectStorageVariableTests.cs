using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local
namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Variable")]
  public sealed class ObjectStorageVariableTests
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

      Assert.That(creature, Is.Not.Null);
      createdTestObjects.Add(creature);

      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableBool>(variableName + "bool"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName + "enum"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableFloat>(variableName + "float"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableGuid>(variableName + "guid"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableInt>(variableName + "int"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableString>(variableName + "string"));
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

      Assert.That(creature, Is.Not.Null);
      createdTestObjects.Add(creature);

      creature.GetObjectVariable<PersistentVariableBool>(variableName + "bool").Value = true;
      creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName + "enum").Value = ValidEnum.TestA;
      creature.GetObjectVariable<PersistentVariableFloat>(variableName + "float").Value = 999f;
      creature.GetObjectVariable<PersistentVariableGuid>(variableName + "guid").Value = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f");
      creature.GetObjectVariable<PersistentVariableInt>(variableName + "int").Value = 506;
      creature.GetObjectVariable<PersistentVariableString>(variableName + "string").Value = "test_string";

      VariableAssert(true, true, creature.GetObjectVariable<PersistentVariableBool>(variableName + "bool"));
      VariableAssert(true, ValidEnum.TestA, creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName + "enum"));
      VariableAssert(true, 999f, creature.GetObjectVariable<PersistentVariableFloat>(variableName + "float"));
      VariableAssert(true, Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"), creature.GetObjectVariable<PersistentVariableGuid>(variableName + "guid"));
      VariableAssert(true, 506, creature.GetObjectVariable<PersistentVariableInt>(variableName + "int"));
      VariableAssert(true, "test_string", creature.GetObjectVariable<PersistentVariableString>(variableName + "string"));
    }

    [Test(Description = "Setting a variable on an object and deleting it returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    [TestCase("\0")]
    [TestCase("\0aaa")]
    public void DeletePersistentVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.That(creature, Is.Not.Null);
      createdTestObjects.Add(creature);

      creature.GetObjectVariable<PersistentVariableBool>(variableName + "bool").Value = true;
      creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName + "enum").Value = ValidEnum.TestA;
      creature.GetObjectVariable<PersistentVariableFloat>(variableName + "float").Value = 999f;
      creature.GetObjectVariable<PersistentVariableGuid>(variableName + "guid").Value = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f");
      creature.GetObjectVariable<PersistentVariableInt>(variableName + "int").Value = 506;
      creature.GetObjectVariable<PersistentVariableString>(variableName + "string").Value = "test_string";

      creature.GetObjectVariable<PersistentVariableBool>(variableName + "bool").Delete();
      creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName + "enum").Delete();
      creature.GetObjectVariable<PersistentVariableFloat>(variableName + "float").Delete();
      creature.GetObjectVariable<PersistentVariableGuid>(variableName + "guid").Delete();
      creature.GetObjectVariable<PersistentVariableInt>(variableName + "int").Delete();
      creature.GetObjectVariable<PersistentVariableString>(variableName + "string").Delete();

      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableBool>(variableName + "bool"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableEnum<ValidEnum>>(variableName + "enum"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableFloat>(variableName + "float"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableGuid>(variableName + "guid"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableInt>(variableName + "int"));
      VariableAssert(false, default, creature.GetObjectVariable<PersistentVariableString>(variableName + "string"));
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

      Assert.That(creature, Is.Not.Null);
      createdTestObjects.Add(creature);

      Assert.That(() =>
      {
        creature.GetObjectVariable<PersistentVariableEnum<InvalidEnumA>>(variableName + "enum").Value = InvalidEnumA.TestA;
      }, Throws.TypeOf<TargetInvocationException>());

      Assert.That(() =>
      {
        creature.GetObjectVariable<PersistentVariableEnum<InvalidEnumB>>(variableName + "enum").Value = InvalidEnumB.TestB;
      }, Throws.TypeOf<TargetInvocationException>());
    }

    private void VariableAssert<T>(bool expectHasValue, T expectedValue, ObjectVariable<T> variable)
    {
      Assert.That(variable, Is.Not.Null, "Created variable was null.");

      if (expectHasValue)
      {
        Assert.That(variable.HasValue, Is.True, "Expected variable to have value, but HasValue returned false.");
        Assert.That(variable.HasNothing, Is.False, "Expected variable to have value, but HasNothing returned true.");
      }
      else
      {
        Assert.That(variable.HasNothing, Is.True, "Expected variable to have no value, but HasNothing returned false.");
        Assert.That(variable.HasValue, Is.False, "Expected variable to have no value, but HasValue returned true.");
      }

      Assert.That(expectedValue, Is.EqualTo(variable.Value));
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
