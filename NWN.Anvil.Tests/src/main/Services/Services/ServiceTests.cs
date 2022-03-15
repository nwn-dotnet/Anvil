using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.Services
{
  [TestFixture(Category = "Services.Services")]
  public sealed class ServiceTests
  {
    [TestCase(Description = "Anvil services are sorted based on binding priority.")]
    public void InjectionServiceInjectsServicesOnStaticProperties()
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
