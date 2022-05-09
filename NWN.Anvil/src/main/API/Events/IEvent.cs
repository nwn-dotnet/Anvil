namespace Anvil.API.Events
{
  public interface IEvent
  {
    public NwObject? Context { get; }
  }
}
