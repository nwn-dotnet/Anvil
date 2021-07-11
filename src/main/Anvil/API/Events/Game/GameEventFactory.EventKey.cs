using System;
using Anvil.API;

namespace NWN.API.Events
{
  public sealed partial class GameEventFactory
  {
    private readonly struct EventKey : IEquatable<EventKey>
    {
      private readonly EventScriptType eventScriptType;
      private readonly uint gameObject;

      public EventKey(EventScriptType eventScriptType, uint gameObject)
      {
        this.eventScriptType = eventScriptType;
        this.gameObject = gameObject;
      }

      public bool Equals(EventKey other)
      {
        return eventScriptType == other.eventScriptType && gameObject == other.gameObject;
      }

      public override bool Equals(object obj)
      {
        return obj is EventKey other && Equals(other);
      }

      public override int GetHashCode()
      {
        return HashCode.Combine((int)eventScriptType, gameObject);
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
