using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.Services
{
  [TestFixture(Category = "Services.Services")]
  public sealed class ServiceTests
  {
    [Test(Description = "Plugin services are sorted based on binding priority.")]
    public void PluginServicesSortedByPriority()
    {
      List<string> serviceNames = new List<string>
      {
        typeof(LowestPriorityService).GetInternalServiceName(),
        typeof(VeryLowPriorityService).GetInternalServiceName(),
        typeof(LowPriorityService).GetInternalServiceName(),
        typeof(BelowNormalPriorityService).GetInternalServiceName(),
        typeof(NormalPriorityService).GetInternalServiceName(),
        typeof(AboveNormalPriorityService).GetInternalServiceName(),
        typeof(HighPriorityService).GetInternalServiceName(),
        typeof(VeryHighPriorityService).GetInternalServiceName(),
        typeof(HighestPriorityService).GetInternalServiceName(),
      };

      serviceNames.Sort();

      Assert.That(serviceNames, Is.EquivalentTo(new[]
      {
        typeof(HighestPriorityService).GetInternalServiceName(),
        typeof(VeryHighPriorityService).GetInternalServiceName(),
        typeof(HighPriorityService).GetInternalServiceName(),
        typeof(AboveNormalPriorityService).GetInternalServiceName(),
        typeof(NormalPriorityService).GetInternalServiceName(),
        typeof(BelowNormalPriorityService).GetInternalServiceName(),
        typeof(LowPriorityService).GetInternalServiceName(),
        typeof(VeryLowPriorityService).GetInternalServiceName(),
        typeof(LowestPriorityService).GetInternalServiceName(),
      }));
    }

    [Test(Description = "Anvil services are sorted based on internal binding priority.")]
    public void InternalServicesSortedByPriority()
    {
      List<string> serviceNames = new List<string>
      {
        typeof(InternalLowestPriorityService).GetInternalServiceName(),
        typeof(InternalVeryLowPriorityService).GetInternalServiceName(),
        typeof(InternalLowPriorityService).GetInternalServiceName(),
        typeof(InternalBelowNormalPriorityService).GetInternalServiceName(),
        typeof(InternalNormalPriorityService).GetInternalServiceName(),
        typeof(InternalAboveNormalPriorityService).GetInternalServiceName(),
        typeof(InternalHighPriorityService).GetInternalServiceName(),
        typeof(InternalVeryHighPriorityService).GetInternalServiceName(),
        typeof(InternalHighestPriorityService).GetInternalServiceName(),
      };

      serviceNames.Sort();

      Assert.That(serviceNames, Is.EquivalentTo(new[]
      {
        typeof(InternalHighestPriorityService).GetInternalServiceName(),
        typeof(InternalVeryHighPriorityService).GetInternalServiceName(),
        typeof(InternalHighPriorityService).GetInternalServiceName(),
        typeof(InternalAboveNormalPriorityService).GetInternalServiceName(),
        typeof(InternalNormalPriorityService).GetInternalServiceName(),
        typeof(InternalBelowNormalPriorityService).GetInternalServiceName(),
        typeof(InternalLowPriorityService).GetInternalServiceName(),
        typeof(InternalVeryLowPriorityService).GetInternalServiceName(),
        typeof(InternalLowestPriorityService).GetInternalServiceName(),
      }));
    }

    [ServiceBindingOptions(InternalBindingPriority.Highest)]
    private sealed class InternalHighestPriorityService {}

    [ServiceBindingOptions(InternalBindingPriority.VeryHigh)]
    private sealed class InternalVeryHighPriorityService {}

    [ServiceBindingOptions(InternalBindingPriority.High)]
    private sealed class InternalHighPriorityService {}

    [ServiceBindingOptions(InternalBindingPriority.AboveNormal)]
    private sealed class InternalAboveNormalPriorityService {}

    [ServiceBindingOptions(InternalBindingPriority.Normal)]
    private sealed class InternalNormalPriorityService {}

    [ServiceBindingOptions(InternalBindingPriority.BelowNormal)]
    private sealed class InternalBelowNormalPriorityService {}

    [ServiceBindingOptions(InternalBindingPriority.Low)]
    private sealed class InternalLowPriorityService {}

    [ServiceBindingOptions(InternalBindingPriority.VeryLow)]
    private sealed class InternalVeryLowPriorityService {}

    [ServiceBindingOptions(InternalBindingPriority.Lowest)]
    private sealed class InternalLowestPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.Highest)]
    private sealed class HighestPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.VeryHigh)]
    private sealed class VeryHighPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.High)]
    private sealed class HighPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.AboveNormal)]
    private sealed class AboveNormalPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.Normal)]
    private sealed class NormalPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.BelowNormal)]
    private sealed class BelowNormalPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.Low)]
    private sealed class LowPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.VeryLow)]
    private sealed class VeryLowPriorityService {}

    [ServiceBindingOptions(BindingPriority = BindingPriority.Lowest)]
    private sealed class LowestPriorityService {}
  }
}
