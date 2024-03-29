using System;
using System.Collections.Generic;
using Anvil.Services;

namespace Anvil.API
{
  /// <summary>
  /// A key/value pair/variable stored on an Object.
  /// </summary>
  public abstract class ObjectVariable
  {
    [Inject]
    private static Lazy<InjectionService> InjectionService { get; set; } = null!;

    /// <summary>
    /// Gets a value indicating whether this variable has no value.
    /// </summary>
    public bool HasNothing => !HasValue;

    /// <summary>
    /// Gets a value indicating whether this variable has a value.
    /// </summary>
    public abstract bool HasValue { get; }

    public string Name { get; private init; } = null!;

    public NwObject Object { get; private init; } = null!;

    /// <summary>
    /// Deletes the value of this variable.
    /// </summary>
    public abstract void Delete();

    internal static T Create<T>(NwObject gameObject, string name) where T : ObjectVariable, new()
    {
      return InjectionService.Value.Inject(new T
      {
        Name = name,
        Object = gameObject,
      });
    }
  }

  public abstract class ObjectVariable<T> : ObjectVariable, IEquatable<ObjectVariable<T>>
  {
    /// <summary>
    /// Gets or sets the current value of this variable. Returns the default value of T if unassigned (null or 0).
    /// </summary>
    public abstract T? Value { get; set; }

    public static bool operator ==(ObjectVariable<T> left, ObjectVariable<T> right)
    {
      return Equals(left, right);
    }

    /// <summary>
    /// Implicit conversion of the value of this variable.
    /// </summary>
    public static implicit operator T?(ObjectVariable<T> value)
    {
      return value.Value;
    }

    public static bool operator !=(ObjectVariable<T> left, ObjectVariable<T> right)
    {
      return !Equals(left, right);
    }

    public bool Equals(ObjectVariable<T>? other)
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

      if (obj.GetType() != GetType())
      {
        return false;
      }

      return Equals((ObjectVariable<T>)obj);
    }

    public override int GetHashCode()
    {
      T? value = Value;
      return value is not null ? EqualityComparer<T>.Default.GetHashCode(value) : 0;
    }
  }
}
