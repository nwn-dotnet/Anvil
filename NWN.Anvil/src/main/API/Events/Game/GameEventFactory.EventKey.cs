using System;

namespace Anvil.API.Events
{
  public sealed partial class GameEventFactory
  {
    private readonly struct EventKey : IEquatable<EventKey>
    {
      public readonly EventScriptType EventScriptType;
      public readonly uint GameObject;

      public EventKey(EventScriptType eventScriptType, uint gameObject)
      {
        this.EventScriptType = eventScriptType;
        this.GameObject = gameObject;
      }

      public static bool operator ==(EventKey left, EventKey right)
      {
        return left.Equals(right);
      }

      public static bool operator !=(EventKey left, EventKey right)
      {
        return !left.Equals(right);
      }

      public bool Equals(EventKey other)
      {
        return EventScriptType == other.EventScriptType && GameObject == other.GameObject;
      }

      public override bool Equals(object? obj)
      {
        return obj is EventKey other && Equals(other);
      }

      public override int GetHashCode()
      {
        return HashCode.Combine((int)EventScriptType, GameObject);
      }
    }
  }
}
