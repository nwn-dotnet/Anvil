﻿using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiSpacerTests
  {
    [Test]
    public void SerializeNuiSpacerReturnsValidJson()
    {
      NuiSpacer element = new NuiSpacer();

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"type":"spacer"}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"type":"spacer"}"""));
    }
  }
}
