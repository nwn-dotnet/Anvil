using NWN;

namespace NWM.API.Constants
{
  public enum RestEventType
  {
    Invalid = NWScript.REST_EVENTTYPE_REST_INVALID,
    Started = NWScript.REST_EVENTTYPE_REST_STARTED,
    Finished = NWScript.REST_EVENTTYPE_REST_FINISHED,
    Cancelled = NWScript.REST_EVENTTYPE_REST_CANCELLED
  }
}