using System;
using System.Reflection;
using System.Text.Json;
using Anvil.Plugins;
using Anvil.Services;
using LightInject;

namespace Anvil.API
{
  internal sealed class JsonCacheService : ICoreService
  {
    private static readonly Type? UpdateHandlerType = typeof(JsonSerializerOptions).Assembly.GetType("System.Text.Json.JsonSerializerOptionsUpdateHandler");
    private static readonly MethodInfo? ClearCacheMethod = UpdateHandlerType?.GetMethod("ClearCache", BindingFlags.Static | BindingFlags.Public);

    [Inject]
    private IServiceManager ServiceManager { get; init; } = null!;

    void ICoreService.Init()
    {
      ServiceManager.OnContainerDispose += OnContainerDispose;
    }

    void ICoreService.Load() {}

    void ICoreService.Shutdown() {}

    void ICoreService.Start() {}

    void ICoreService.Unload() {}

    private void OnContainerDispose(IServiceContainer container, Plugin? plugin, bool immediateDispose)
    {
      ClearCacheMethod?.Invoke(null, [null]);
    }
  }
}
