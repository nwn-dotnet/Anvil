using NWN.Services;

namespace NWN.API.Events
{
  public interface IEventScriptResult : IEvent
  {
    ScriptHandleResult Result { get; }
  }
}
