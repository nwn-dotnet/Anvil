namespace Anvil.Services
{
  internal enum LifeCycleEvent
  {
    Unhandled = 0,
    ModuleLoad,
    DestroyServer,
    DestroyServerAfter,
  }
}
