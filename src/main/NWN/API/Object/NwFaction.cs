using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public sealed class NwFaction : IEquatable<NwFaction>
  {
    internal readonly NwGameObject GameObject;

    public NwFaction(NwGameObject gameObject)
    {
      this.GameObject = gameObject;
    }

    /// <summary>
    /// Gets the most common type of class among the members of this faction/party.
    /// </summary>
    public ClassType MostFrequentClass
    {
      get => (ClassType) NWScript.GetFactionMostFrequentClass(GameObject);
    }

    public int AverageLevel
    {
      get => NWScript.GetFactionAverageLevel(GameObject);
    }

    public int AverageXP
    {
      get => NWScript.GetFactionAverageXP(GameObject);
    }

    /// <summary>
    /// Get the total amount of gold held by all members of this party.
    /// </summary>
    public int Gold
    {
      get => NWScript.GetFactionGold(GameObject);
    }

    /// <summary>
    /// Gets the members in this faction.
    /// </summary>
    /// <typeparam name="T">The types of members to get.</typeparam>
    /// <returns></returns>
    public IEnumerable<T> GetMembers<T>() where T : NwGameObject
    {
      int pcOnly = (typeof(T) == typeof(NwPlayer)).ToInt();

      for (uint obj = NWScript.GetFirstFactionMember(GameObject, pcOnly);
        obj != NWScript.OBJECT_INVALID;
        obj = NWScript.GetNextFactionMember(GameObject, pcOnly))
      {
        T next = obj.ToNwObject<T>();
        if (next != null)
        {
          yield return next;
        }
      }
    }

    public bool Equals(NwFaction other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return NWScript.GetFactionEqual(GameObject, other.GameObject).ToBool();
    }

    public override bool Equals(object obj)
    {
      return ReferenceEquals(this, obj) || obj is NwFaction other && Equals(other);
    }

    public override int GetHashCode()
    {
      return (GameObject != null ? GameObject.GetHashCode() : 0);
    }

    public static bool operator ==(NwFaction left, NwFaction right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(NwFaction left, NwFaction right)
    {
      return !Equals(left, right);
    }
  }
}