using System;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.Services
{
  [TestFixture(Category = "Services.Services")]
  public sealed class LazyServiceTests
  {
    [Inject]
    private static LazyServiceConsumerConstructor StaticLazyServiceConsumerConstructor { get; set; } = null!;

    [Inject]
    private static LazyServiceConsumerProperty StaticLazyServiceConsumerProperty { get; set; } = null!;

    [Inject]
    private static Lazy<LazyServicePropertyStatic> StaticLazyServicePropertyStatic { get; set; } = null!;

    [Test(Description = "A lazy service is not initialized until used when declared as a dependency in a constructor.")]
    public void ConstructorLazyServiceDependencyInitializedWhenUsed()
    {
      Assert.That(LazyServiceConstructor.Created, Is.EqualTo(false), "The lazy service was already initialized");
      Assert.That(StaticLazyServiceConsumerConstructor.LazyServiceConstructor.Value, Is.Not.Null, "The lazy service was not initialized after being used.");
      Assert.That(LazyServiceConstructor.Created, Is.EqualTo(true), "The lazy service was not initialized after being used.");
    }

    [Test(Description = "A lazy service is not initialized until used when declared as a dependency in a property.")]
    public void PropertyLazyServiceDependencyInitializedWhenUsed()
    {
      Assert.That(LazyServiceProperty.Created, Is.EqualTo(false), "The lazy service was already initialized");
      Assert.That(StaticLazyServiceConsumerProperty.LazyServiceProperty.Value, Is.Not.Null, "The lazy service was not initialized after being used.");
      Assert.That(LazyServiceProperty.Created, Is.EqualTo(true), "The lazy service was not initialized after being used.");
    }

    [Test(Description = "A lazy service is not initialized until used when declared as a dependency in a static property.")]
    public void StaticPropertyLazyServiceDependencyInitializedWhenUsed()
    {
      Assert.That(LazyServicePropertyStatic.Created, Is.EqualTo(false), "The lazy service was already initialized");
      Assert.That(StaticLazyServicePropertyStatic.Value, Is.Not.Null, "The lazy service was not initialized after being used.");
      Assert.That(LazyServicePropertyStatic.Created, Is.EqualTo(true), "The lazy service was not initialized after being used.");
    }

    [ServiceBinding(typeof(LazyServiceConsumerConstructor))]
    internal sealed class LazyServiceConsumerConstructor
    {
      public Lazy<LazyServiceConstructor> LazyServiceConstructor { get; }

      public LazyServiceConsumerConstructor(Lazy<LazyServiceConstructor> lazyServiceConstructor)
      {
        LazyServiceConstructor = lazyServiceConstructor;
      }
    }

    [ServiceBinding(typeof(LazyServiceConsumerProperty))]
    internal sealed class LazyServiceConsumerProperty
    {
      [Inject]
      public Lazy<LazyServiceProperty> LazyServiceProperty { get; init; } = null!;
    }

    [ServiceBinding(typeof(LazyServiceProperty))]
    [ServiceBindingOptions(Lazy = true)]
    internal sealed class LazyServiceProperty
    {
      public static bool Created = false;

      public LazyServiceProperty()
      {
        Created = true;
      }
    }

    [ServiceBinding(typeof(LazyServicePropertyStatic))]
    [ServiceBindingOptions(Lazy = true)]
    internal sealed class LazyServicePropertyStatic
    {
      public static bool Created = false;

      public LazyServicePropertyStatic()
      {
        Created = true;
      }
    }

    [ServiceBinding(typeof(LazyServiceConstructor))]
    [ServiceBindingOptions(Lazy = true)]
    internal sealed class LazyServiceConstructor
    {
      public static bool Created = false;

      public LazyServiceConstructor()
      {
        Created = true;
      }
    }
  }
}
