﻿using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiLabelTests
  {
    [Test]
    public void SerializeNuiLabelReturnsValidJson()
    {
      NuiLabel element = new NuiLabel("Label")
      {
        HorizontalAlign = NuiHAlign.Center,
        VerticalAlign = NuiVAlign.Middle,
      };

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"text_halign":0,"value":"Label","type":"label","text_valign":0}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"text_halign":0,"value":"Label","type":"label","text_valign":0}"""));
    }
  }
}
