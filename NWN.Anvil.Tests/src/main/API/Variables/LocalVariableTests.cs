using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local
namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class LocalVariableTests
  {
    private readonly List<NwGameObject> createdTestObjects = [];

    [Test(Description = "Getting a non-existing variable on an object returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    public void GetMissingLocalVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.That(creature, Is.Not.Null);
      createdTestObjects.Add(creature!);

      VariableAssert(false, default, creature!.GetObjectVariable<LocalVariableBool>(variableName + "bool"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName + "enum"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableFloat>(variableName + "float"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableGuid>(variableName + "guid"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableInt>(variableName + "int"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableString>(variableName + "string"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableStruct<ExampleSerializable>>(variableName + "string"));
    }

    [Test(Description = "Setting/getting a variable on an object returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    public void GetValidLocalVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.That(creature, Is.Not.Null);
      createdTestObjects.Add(creature!);

      creature!.GetObjectVariable<LocalVariableBool>(variableName + "bool").Value = true;
      creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName + "enum").Value = ValidEnum.TestA;
      creature.GetObjectVariable<LocalVariableFloat>(variableName + "float").Value = 999f;
      creature.GetObjectVariable<LocalVariableGuid>(variableName + "guid").Value = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f");
      creature.GetObjectVariable<LocalVariableInt>(variableName + "int").Value = 506;
      creature.GetObjectVariable<LocalVariableString>(variableName + "string").Value = "test_string";
      ExampleSerializable serializable = new ExampleSerializable
      {
        Value = 1,
        Value2 = "test",
      };
      creature.GetObjectVariable<LocalVariableStruct<ExampleSerializable>>(variableName + "struct").Value = serializable;

      VariableAssert(true, true, creature.GetObjectVariable<LocalVariableBool>(variableName + "bool"));
      VariableAssert(true, ValidEnum.TestA, creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName + "enum"));
      VariableAssert(true, 999f, creature.GetObjectVariable<LocalVariableFloat>(variableName + "float"));
      VariableAssert(true, Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"), creature.GetObjectVariable<LocalVariableGuid>(variableName + "guid"));
      VariableAssert(true, 506, creature.GetObjectVariable<LocalVariableInt>(variableName + "int"));
      VariableAssert(true, "test_string", creature.GetObjectVariable<LocalVariableString>(variableName + "string"));
      VariableAssert(true, serializable, creature.GetObjectVariable<LocalVariableStruct<ExampleSerializable>>(variableName + "struct"));
    }

    [Test(Description = "Setting a variable on an object and deleting it returns a variable object with the correct properties.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    public void DeleteLocalVariablePropertiesValid(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.That(creature, Is.Not.Null);
      createdTestObjects.Add(creature!);

      creature!.GetObjectVariable<LocalVariableBool>(variableName + "bool").Value = true;
      creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName + "enum").Value = ValidEnum.TestA;
      creature.GetObjectVariable<LocalVariableFloat>(variableName + "float").Value = 999f;
      creature.GetObjectVariable<LocalVariableGuid>(variableName + "guid").Value = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f");
      creature.GetObjectVariable<LocalVariableInt>(variableName + "int").Value = 506;
      creature.GetObjectVariable<LocalVariableString>(variableName + "string").Value = "test_string";
      ExampleSerializable serializable = new ExampleSerializable
      {
        Value = 1,
        Value2 = "test",
      };
      creature.GetObjectVariable<LocalVariableStruct<ExampleSerializable>>(variableName + "struct").Value = serializable;

      creature.GetObjectVariable<LocalVariableBool>(variableName + "bool").Delete();
      creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName + "enum").Delete();
      creature.GetObjectVariable<LocalVariableFloat>(variableName + "float").Delete();
      creature.GetObjectVariable<LocalVariableGuid>(variableName + "guid").Delete();
      creature.GetObjectVariable<LocalVariableInt>(variableName + "int").Delete();
      creature.GetObjectVariable<LocalVariableString>(variableName + "string").Delete();
      creature.GetObjectVariable<LocalVariableStruct<ExampleSerializable>>(variableName + "struct").Delete();

      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableBool>(variableName + "bool"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableEnum<ValidEnum>>(variableName + "enum"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableFloat>(variableName + "float"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableGuid>(variableName + "guid"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableInt>(variableName + "int"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableString>(variableName + "string"));
      VariableAssert(false, default, creature.GetObjectVariable<LocalVariableStruct<ExampleSerializable>>(variableName + "struct"));
    }

    [Test(Description = "Attempting to create an object enum variable with an incorrect size throws an exception.")]
    [TestCase("aaabbb")]
    [TestCase("123")]
    [TestCase(",;'.-2=,'\"")]
    [TestCase("__\n")]
    public void GetInvalidEnumVariableThrowsException(string variableName)
    {
      Location startLocation = NwModule.Instance.StartingLocation;
      NwCreature? creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, startLocation);

      Assert.That(creature, Is.Not.Null);
      createdTestObjects.Add(creature!);

      Assert.That(() =>
      {
        creature!.GetObjectVariable<LocalVariableEnum<InvalidEnumA>>(variableName + "enum").Value = InvalidEnumA.TestA;
      }, Throws.TypeOf<TargetInvocationException>());

      Assert.That(() =>
      {
        creature!.GetObjectVariable<LocalVariableEnum<InvalidEnumB>>(variableName + "enum").Value = InvalidEnumB.TestB;
      }, Throws.TypeOf<TargetInvocationException>());
    }

    private void VariableAssert<T>(bool expectHasValue, T? expectedValue, ObjectVariable<T> variable)
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

    [Serializable]
    private record ExampleSerializable
    {
      public int Value { get; set; }
      public string? Value2 { get; set; }
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
