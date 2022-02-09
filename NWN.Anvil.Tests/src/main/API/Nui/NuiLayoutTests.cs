using System.Collections.Generic;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Nui")]
  public sealed class NuiLayoutTests
  {
    [Test(Description = "Serializing a NuiGroup creates a valid JSON structure.")]
    public void SerializeNuiGroupReturnsValidJsonStructure()
    {
      NuiGroup nuiGroup = new NuiGroup
      {
        Id = "test_group",
        Aspect = 1.5f,
        Border = true,
        Enabled = new NuiBind<bool>("enabled_bind"),
        Height = 10f,
        Margin = 2f,
        Padding = 3f,
        ForegroundColor = new NuiBind<Color>("color_bind"),
        Scrollbars = NuiScrollbars.Both,
        Tooltip = "test_tooltip",
        Width = 100f,
        Visible = false,
        Layout = new NuiColumn
        {
          Children = new List<NuiElement>
          {
            new NuiLabel("Test"),
          },
        },
      };

      Assert.That(JsonUtility.ToJson(nuiGroup), Is.EqualTo(@"{""border"":true,""scrollbars"":3,""type"":""group"",""children"":[{""type"":""col"",""children"":[{""text_halign"":1,""value"":""Test"",""type"":""label"",""text_valign"":1}]}],""aspect"":1.5,""enabled"":{""bind"":""enabled_bind""},""foreground_color"":{""bind"":""color_bind""},""height"":10.0,""id"":""test_group"",""margin"":2.0,""padding"":3.0,""tooltip"":""test_tooltip"",""visible"":false,""width"":100.0}"));
    }

    [Test(Description = "Serializing a NuiColumn creates a valid JSON structure.")]
    public void SerializeNuiColumnReturnsValidJsonStructure()
    {
      NuiColumn nuiColumn = new NuiColumn
      {
        Id = "test_column",
        Aspect = 1.5f,
        Enabled = new NuiBind<bool>("enabled_bind"),
        Height = 10f,
        Margin = 2f,
        Padding = 3f,
        ForegroundColor = new NuiBind<Color>("color_bind"),
        Tooltip = "test_tooltip",
        Width = 100f,
        Visible = false,
        Children = new List<NuiElement>
        {
          new NuiLabel("test"),
          new NuiRow(),
        },
      };

      Assert.That(JsonUtility.ToJson(nuiColumn), Is.EqualTo(@"{""type"":""col"",""children"":[{""text_halign"":1,""value"":""test"",""type"":""label"",""text_valign"":1},{""type"":""row"",""children"":[]}],""aspect"":1.5,""enabled"":{""bind"":""enabled_bind""},""foreground_color"":{""bind"":""color_bind""},""height"":10.0,""id"":""test_column"",""margin"":2.0,""padding"":3.0,""tooltip"":""test_tooltip"",""visible"":false,""width"":100.0}"));
    }

    [Test(Description = "Serializing a NuiRow creates a valid JSON structure.")]
    public void SerializeNuiRowReturnsValidJsonStructure()
    {
      NuiRow nuiRow = new NuiRow
      {
        Id = "test_row",
        Aspect = 1.5f,
        Enabled = new NuiBind<bool>("enabled_bind"),
        Height = 10f,
        Margin = 2f,
        Padding = 3f,
        ForegroundColor = new NuiBind<Color>("color_bind"),
        Tooltip = "test_tooltip",
        Width = 100f,
        Visible = false,
        Children = new List<NuiElement>
        {
          new NuiLabel("test"),
          new NuiRow(),
        },
      };

      Assert.That(JsonUtility.ToJson(nuiRow), Is.EqualTo(@"{""type"":""row"",""children"":[{""text_halign"":1,""value"":""test"",""type"":""label"",""text_valign"":1},{""type"":""row"",""children"":[]}],""aspect"":1.5,""enabled"":{""bind"":""enabled_bind""},""foreground_color"":{""bind"":""color_bind""},""height"":10.0,""id"":""test_row"",""margin"":2.0,""padding"":3.0,""tooltip"":""test_tooltip"",""visible"":false,""width"":100.0}"));
    }
  }
}
