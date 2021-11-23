namespace Anvil.Services
{
  internal enum InternalBindingPriority
  {
    Highest = -99999,
    Core = -90000,
    API = -80000,
    Normal = 0,
    Lowest = 99999,
  }
}
