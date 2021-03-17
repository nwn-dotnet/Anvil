using NWN.Services;

namespace NWN.API.Events
{
  public readonly struct HookInfo
  {
    public uint Address { get; init; }

    public int Order { get; init; }

    public HookInfo(uint address, int order)
    {
      Address = address;
      Order = order;
    }

    public static implicit operator HookInfo(uint address)
    {
      return new HookInfo
      {
        Address = address,
        Order = HookOrder.Default
      };
    }
  }
}
