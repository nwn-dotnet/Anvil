using System;

namespace Anvil.API.Events
{
  public sealed partial class GameEventFactory
  {
    private readonly struct EventKey(EventScriptType eventScriptType, uint gameObject) : IEquatable<EventKey>
    {
      public readonly EventScriptType EventScriptType = eventScriptType;
      public readonly uint GameObject = gameObject;

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
