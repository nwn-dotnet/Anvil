using System;
using NWN.Core.NWNX;

namespace NWN.API
{
  /// <summary>
  /// Represents a local variable stored on an Object.
  /// </summary>
  public class LocalVariable<T> : IEquatable<LocalVariable<T>>
  {
    public string Name { get; private set; }
    public NwObject Object { get; private set; }

    private ILocalVariableConverter<T> converter;

    private LocalVariable() {}

    internal static LocalVariable<T> Create(NwObject instance, string name)
    {
      LocalVariable<T> variable = new LocalVariable<T>();
      variable.Name = name;
      variable.Object = instance;
      variable.converter = VariableConverterManager.GetLocalConverter<T>();

      return variable;
    }

    /// <summary>
    /// The current value of this variable, otherwise the default value if unassigned (null or 0).
    /// </summary>
    public T Value
    {
      get => converter.GetLocal(Object, Name);
      set => converter.SetLocal(Object, Name, value);
    }

    /// <summary>
    /// True if this variable has no value, otherwise false.
    /// </summary>
    public bool HasNothing => !HasValue;

    /// <summary>
    /// True if this variable has a value, otherwise false.
    /// </summary>
    public bool HasValue
    {
      get
      {
        int localCount = ObjectPlugin.GetLocalVariableCount(Object);
        for (int i = 0; i < localCount; i++)
        {
          LocalVariable variable = ObjectPlugin.GetLocalVariable(Object, i);
          if (variable.key == Name)
          {
            return true;
          }
        }

        return false;
      }
    }

    /// <summary>
    /// Implicit conversion of the value of this variable.
    /// </summary>
    public static implicit operator T(LocalVariable<T> value)
    {
      return value.Value;
    }

    /// <summary>
    /// Deletes the value of this variable.
    /// </summary>
    public void Delete() => converter.ClearLocal(Object, Name);

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