using System;

namespace NWN.API
{
  public abstract class LocalVariable<T> : IEquatable<LocalVariable<T>>
  {
    public readonly string Name;
    public readonly uint Object;

    public abstract T Value { get; set; }

    public static implicit operator T(LocalVariable<T> value)
    {
      return value.Value;
    }

    protected LocalVariable(NwObject instance, string name)
    {
      Name = name;
      Object = instance;
    }

    public abstract void Delete();

    public bool Equals(LocalVariable<T> other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return Equals(Value, other.Value);
    }

    public override bool Equals(object obj)
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

      return Equals((LocalVariable<T>) obj);
    }

    public override int GetHashCode()
    {
      return (Value != null ? Value.GetHashCode() : 0);
    }

    public static bool operator ==(LocalVariable<T> left, LocalVariable<T> right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(LocalVariable<T> left, LocalVariable<T> right)
    {
      return !Equals(left, right);
    }
  }
}