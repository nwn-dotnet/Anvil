using System;
using LightInject;
using NLog;
using LogLevel = LightInject.LogLevel;

namespace Anvil.Services
{
  internal sealed class AnvilContainerFactory
  {
    private readonly IServiceManager serviceManager;

    private readonly ContainerOptions containerOptions = new ContainerOptions
    {
      EnablePropertyInjection = true,
      EnableCurrentScope = false,
      EnableVariance = false,
      DefaultServiceSelector = SelectHighestPriorityService,
      LogFactory = CreateLogHandler,
    };

    public AnvilContainerFactory(IServiceManager serviceManager)
    {
      this.serviceManager = serviceManager;
    }

    public ServiceContainer CreateContainer(IServiceContainer? parentContainer = null)
    {
      ServiceContainer serviceContainer = new ServiceContainer(containerOptions);
      SetupInjectPropertySelector(serviceContainer);

      serviceContainer.RegisterInstance(typeof(IServiceManager), serviceManager);

      if (parentContainer != null)
      {
        serviceContainer.RegisterParentContainer(parentContainer);
      }

      return serviceContainer;
    }

    private static string SelectHighestPriorityService(string[] services)
    {
      // Services are sorted in priority order.
      // So we just return the first service.
      return services[0];
    }

    private static void SetupInjectPropertySelector(ServiceContainer serviceContainer)
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.InstanceOnly);
      serviceContainer.PropertyDependencySelector = new InjectPropertyDependencySelector(propertySelector);
    }

    private static Action<LogEntry> CreateLogHandler(Type type)
    {
      Logger logger = LogManager.GetLogger(type.FullName);
      return entry =>
      {
        switch (entry.Level)
        {
          case LogLevel.Info:
            logger.Debug(entry.Message);
            break;
          case LogLevel.Warning:
            logger.Warn(entry.Message);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      };
    }
  }
}
