using System;
using System.Collections.Generic;
using NWN.Core;

namespace Anvil.API
{
  public abstract class CampaignVariable
  {
    public string Campaign { get; private init; } = null!;

    public string Name { get; private init; } = null!;

    public NwPlayer? Player { get; private init; }

    /// <summary>
    /// Deletes the value of this variable.
    /// </summary>
    public abstract void Delete();

    internal static T Create<T>(string campaign, string name, NwPlayer? player = null) where T : CampaignVariable, new()
    {
      T variable = new T
      {
        Campaign = campaign,
        Name = name,
        Player = player,
      };

      return variable;
    }
  }

  public abstract class CampaignVariable<T> : CampaignVariable, IEquatable<CampaignVariable<T>>
  {
    /// <summary>
    /// Gets or sets the current value of this variable. Returns the default value of T if unassigned (null or 0).
    /// </summary>
    public abstract T Value { get; set; }

    /// <summary>
    /// Implicit conversion of the value of this variable.
    /// </summary>
    public static implicit operator T(CampaignVariable<T> value)
    {
      return value.Value;
    }

    public override void Delete()
    {
      NWScript.DeleteCampaignVariable(Campaign, Name, Player?.ControlledCreature);
    }

    public bool Equals(CampaignVariable<T>? other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj.GetType() != this.GetType())
      {
        return false;
      }

      return Equals((CampaignVariable<T>)obj);
    }

    public override int GetHashCode()
    {
      T value = Value;
      return value is not null ? EqualityComparer<T>.Default.GetHashCode(value) : 0;
    }

    public static bool operator ==(CampaignVariable<T>? left, CampaignVariable<T>? right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(CampaignVariable<T>? left, CampaignVariable<T>? right)
    {
      return !Equals(left, right);
    }
  }
}
