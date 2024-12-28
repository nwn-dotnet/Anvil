using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Anvil.API;
using LightInject;
using NLog;

namespace Anvil.Services
{
  internal static class AnvilContainerExtensions
  {
    private static readonly Logger Log = LogManager.GetLogger(typeof(AnvilServiceManager).FullName);

    public static void RegisterCoreService<T>(this IServiceContainer container) where T : ICoreService
    {
      Type bindToType = typeof(T);
      if (bindToType.IsAbstract || bindToType.ContainsGenericParameters)
      {
        return;
      }

      ServiceBindingOptionsAttribute? options = bindToType.GetCustomAttribute<ServiceBindingOptionsAttribute>();
      RegisterAnvilService(container, bindToType, new[] { bindToType, typeof(ICoreService) }, options);
    }

    public static void RegisterAnvilService(this IServiceContainer serviceContainer, Type bindToType, IEnumerable<Type> bindFromTypes, ServiceBindingOptionsAttribute? options)
    {
      string serviceName = bindToType.GetInternalServiceName();

      PerContainerLifetime lifeTime = new PerContainerLifetime();
      RegisterExplicitBindings(serviceContainer, bindToType, bindFromTypes, serviceName, lifeTime);

      if (options is not { Lazy: true })
      {
        RegisterImplicitBindings(serviceContainer, bindToType, serviceName, lifeTime);
      }

      Log.Info("Registered service {Service}", bindToType.FullName);
    }

    public static void RegisterParentContainer(this IServiceContainer container, IServiceContainer parent)
    {
      container.RegisterFallback(CanResolveFromParentContainer, ResolveFromParentContainer);
      return;

      bool CanResolveFromParentContainer(Type serviceType, string serviceName)
      {
        return !IsContainerMessageType(serviceType) && parent.CanGetInstance(serviceType, serviceName);
      }

      object ResolveFromParentContainer(ServiceRequest request)
      {
        return parent.GetInstance(request.ServiceType, request.ServiceName);
      }
    }

    public static void ConstructAllServices(this IServiceContainer container)
    {
      container.GetAllInstances<object>();
    }

    public static bool IsAnvilService(this Type type, [NotNullWhen(true)] out ServiceBindingAttribute[]? bindings, out ServiceBindingOptionsAttribute? options)
    {
      bindings = null;
      options = null;

      if (!type.IsClass || type.IsAbstract || type.ContainsGenericParameters)
      {
        return false;
      }

      bindings = type.GetCustomAttributes<ServiceBindingAttribute>();
      if (bindings.Length == 0)
      {
        bindings = null;
        return false;
      }

      options = type.GetCustomAttribute<ServiceBindingOptionsAttribute>();
      return true;
    }

    private static void RegisterExplicitBindings(IServiceContainer serviceContainer, Type bindToType, IEnumerable<Type> bindFromTypes, string serviceName, ILifetime lifeTime)
    {
      foreach (Type binding in bindFromTypes)
      {
        serviceContainer.Register(binding, bindToType, serviceName, lifeTime);
        Log.Debug("Bind {BindFrom} -> {BindTo}", binding.FullName, bindToType.FullName);
      }
    }

    private static void RegisterImplicitBindings(IServiceContainer serviceContainer, Type bindTo, string serviceName, ILifetime lifeTime)
    {
      serviceContainer.Register(typeof(object), bindTo, serviceName, lifeTime);

      // Message types
      if (bindTo.IsAssignableTo(typeof(IInitializable)))
      {
        serviceContainer.Register(typeof(IInitializable), bindTo, serviceName, lifeTime);
      }

      if (bindTo.IsAssignableTo(typeof(ILateDisposable)))
      {
        serviceContainer.Register(typeof(ILateDisposable), bindTo, serviceName, lifeTime);
      }
    }

    private static bool IsContainerMessageType(Type type)
    {
      return type == typeof(IInitializable) ||
        type == typeof(IUpdateable) ||
        type == typeof(ILateDisposable);
    }
  }
}
