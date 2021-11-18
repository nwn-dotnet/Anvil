using Anvil.Services;

namespace Anvil.Internal
{
  internal interface IServerLifeCycleEventHandler
  {
    void HandleLifeCycleEvent(LifeCycleEvent eventType);
  }
}
