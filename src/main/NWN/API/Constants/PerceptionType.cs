using NWN.Core;

namespace NWN.API.Constants
{
  public enum PerceptionType
  {
    SeenAndHeard = NWScript.PERCEPTION_SEEN_AND_HEARD,
    NotSeenAndNotHeard = NWScript.PERCEPTION_NOT_SEEN_AND_NOT_HEARD,
    HeardAndNotSeen = NWScript.PERCEPTION_HEARD_AND_NOT_SEEN,
    SeenAndNotHeard = NWScript.PERCEPTION_SEEN_AND_NOT_HEARD,
    NotHeard = NWScript.PERCEPTION_NOT_HEARD,
    Heard = NWScript.PERCEPTION_HEARD,
    NotSeen = NWScript.PERCEPTION_NOT_SEEN,
    Seen = NWScript.PERCEPTION_SEEN
  }
}
