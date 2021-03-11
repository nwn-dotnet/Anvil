namespace NWN.API.Events
{
  public interface IEvent
  {
    internal bool HasContext { get; }

    internal NwObject Context { get; }
  }
}
