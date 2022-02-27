using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.Services
{
  [TestFixture(Category = "Services.Services")]
  public sealed class ServiceTests
  {
    private readonly MethodInfo getServiceNameMethod = typeof(ReflectionExtensions).GetMethod("GetInternalServiceName", BindingFlags.Static | BindingFlags.NonPublic);

    [TestCase(Description = "Anvil services are sorted based on binding priority.")]
    public void InjectionServiceInjectsServicesOnStaticProperties()
    {
      List<string> serviceNames = new List<string>
      {
        GetServiceName(typeof(LowestPriorityService)),
        GetServiceName(typeof(VeryLowPriorityService)),
        GetServiceName(typeof(LowPriorityService)),
        GetServiceName(typeof(BelowNormalPriorityService)),
        GetServiceName(typeof(NormalPriorityService)),
        GetServiceName(typeof(AboveNormalPriorityService)),
        GetServiceName(typeof(HighPriorityService)),
        GetServiceName(typeof(VeryHighPriorityService)),
        GetServiceName(typeof(HighestPriorityService)),
      };

      serviceNames.Sort();

      Assert.That(serviceNames, Is.EquivalentTo(new[]
      {
        GetServiceName(typeof(HighestPriorityService)),
        GetServiceName(typeof(VeryHighPriorityService)),
        GetServiceName(typeof(HighPriorityService)),
        GetServiceName(typeof(AboveNormalPriorityService)),
        GetServiceName(typeof(NormalPriorityService)),
        GetServiceName(typeof(BelowNormalPriorityService)),
        GetServiceName(typeof(LowPriorityService)),
        GetServiceName(typeof(VeryLowPriorityService)),
        GetServiceName(typeof(LowestPriorityService)),
      }));
    }

    private string GetServiceName(Type type)
    {
      return getServiceNameMethod.Invoke(null, new object[] { type })?.ToString();
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
