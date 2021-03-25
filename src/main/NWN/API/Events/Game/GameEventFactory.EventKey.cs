using System;
using NWN.API.Constants;

namespace NWN.API.Events
{
  public sealed partial class GameEventFactory
  {
    private readonly struct EventKey : IEquatable<EventKey>
    {
      public readonly EventScriptType EventScriptType;
      public readonly uint Object;

      public EventKey(EventScriptType eventScriptType, uint nwObject)
      {
        EventScriptType = eventScriptType;
        Object = nwObject;
      }

      public bool Equals(EventKey other)
      {
        return EventScriptType == other.EventScriptType && Object == other.Object;
      }

      public override bool Equals(object obj)
      {
        return obj is EventKey other && Equals(other);
      }

      public override int GetHashCode()
      {
        return HashCode.Combine((int)EventScriptType, Object);
      }

      public static bool operator ==(EventKey left, EventKey right)
      {
        return left.Equals(right);
      }

      public static bool operator !=(EventKey left, EventKey right)
      {
        return !left.Equals(right);
      }
    }
  }
}
