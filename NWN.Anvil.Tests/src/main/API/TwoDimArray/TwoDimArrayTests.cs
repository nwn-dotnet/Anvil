using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;
using NWN.Native.API;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.TwoDimArray")]
  public sealed class TwoDimArrayTests
  {
    [Inject]
    private static ResourceManager ResourceManager { get; set; }

    private readonly List<string> createdTempResources = new List<string>();

    [Test(Description = "A user-specified 2da resource returns correct values.")]
    public void Custom2daReturnsValidValues()
    {
      string twoDimArray =
        @"2DA V2.0

    LABEL    TESTSTR    TESTINT    TESTFLOAT
0   Test0    ""Test 0""   0         0.0f 
1   Test1    ""Test 1""   1         1.0f";

      string resourceName = "testtemp.2da";
      ResourceManager.WriteTempResource(resourceName, StringHelper.Cp1252Encoding.GetBytes(twoDimArray));
      createdTempResources.Add(resourceName);

      TwoDimArray array = new TwoDimArray(resourceName);

      Assert.That(array.RowCount, Is.EqualTo(2));
      Assert.That(array.ColumnCount, Is.EqualTo(4));
      Assert.That(array.Columns, Is.EquivalentTo(new[] { "label", "teststr", "testint", "testfloat" }));

      Assert.That(array.GetString(0, "TESTSTR"), Is.EqualTo("Test 0"));
      Assert.That(array.GetString(1, "TESTSTR"), Is.EqualTo("Test 1"));
      Assert.That(array.GetInt(0, "TESTINT"), Is.EqualTo(0));
      Assert.That(array.GetInt(1, "TESTINT"), Is.EqualTo(1));
      Assert.That(array.GetFloat(0, "TESTFLOAT"), Is.EqualTo(0f));
      Assert.That(array.GetFloat(1, "TESTFLOAT"), Is.EqualTo(1f));
    }

    [Test(Description = "An invalid 2da ResRef throws an exception.")]
    public void Invalid2daThrowsException()
    {
      Assert.That(() =>
      {
        TwoDimArray _ = new TwoDimArray("invalidtest");
      }, Throws.Exception.TypeOf<ArgumentException>());
    }

    [TearDown]
    public void CleanupCreatedResources()
    {
      foreach (string resource in createdTempResources)
      {
        ResourceManager.DeleteTempResource(resource);
      }

      createdTempResources.Clear();
    }
  }
}
