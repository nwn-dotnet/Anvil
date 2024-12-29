using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.Services
{
  [TestFixture(Category = "Services.Services")]
  public sealed class InjectionServiceTests
  {
    [Inject]
    private static InjectionService? InjectionService { get; set; }

    [Inject]
    private static HookService? StaticHookService { get; set; }

    [Inject]
    private static InjectionTestService? StaticInjectionTestService { get; set; }

    [Test(Description = "Anvil services are injected on static properties.")]
    public void InjectionServiceInjectsServicesOnStaticProperties()
    {
      Assert.That(InjectionService, Is.Not.Null, "The standard anvil service was not injected.");
      Assert.That(StaticHookService, Is.Not.Null, "The core anvil service was not injected.");
      Assert.That(StaticInjectionTestService, Is.Not.Null, "The plugin service was not injected.");
    }

    [Test(Description = "The injection service injects dependencies into a specified object.")]
    public void InjectionServiceInjectsInstanceDependencies()
    {
      InjectionTest injectionTest = new InjectionTest();
      Assert.That(injectionTest.EventService, Is.Null);
      Assert.That(injectionTest.HookService, Is.Null);
      Assert.That(injectionTest.InjectionTestService, Is.Null);

      InjectionService!.Inject(injectionTest);

      Assert.That(injectionTest.EventService, Is.Not.Null);
      Assert.That(injectionTest.HookService, Is.Not.Null);
      Assert.That(injectionTest.InjectionTestService, Is.Not.Null);
    }

    [Test(Description = "Services with inject properties are implicitly injected.")]
    public void InjectionServiceInjectsServiceDependencies()
    {
      Assert.That(StaticInjectionTestService?.ResourceManager, Is.Not.Null, "A constructor dependency was not injected.");
      Assert.That(StaticInjectionTestService?.ChatService, Is.Null, "A property was injected when it shouldn't have.");
      Assert.That(StaticInjectionTestService?.EventService, Is.Not.Null, "A property dependency was not injected.");
      Assert.That(StaticInjectionTestService?.HookService, Is.Not.Null, "A property dependency was not injected.");
    }

    private sealed class InjectionTest
    {
      [Inject]
      internal EventService? EventService { get; init; }

      [Inject]
      internal HookService? HookService { get; init; }

      [Inject]
      internal InjectionTestService? InjectionTestService { get; init; }
    }

    [ServiceBinding(typeof(InjectionTestService))]
    internal sealed class InjectionTestService(ResourceManager resourceManager)
    {
      public ResourceManager? ResourceManager { get; } = resourceManager;

      // Not injected with attribute or initialized from constructor, expected to be null.
      // ReSharper disable once UnassignedGetOnlyAutoProperty
      public ChatService? ChatService { get; }

      [Inject]
      internal EventService? EventService { get; init; }

      [Inject]
      internal HookService? HookService { get; init; }
    }
  }
}
