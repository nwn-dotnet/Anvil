using System;

namespace NWN.API
{
  public class CampaignVariable<T> : IEquatable<CampaignVariable<T>>
  {
    public string Campaign { get; private set; }
    public string Name { get; private set; }
    public NwPlayer Player { get; private set; }

    private ICampaignVariableConverter<T> converter;

    private CampaignVariable() {}

    internal static CampaignVariable<T> Create(string campaign, string name, NwPlayer player = null)
    {
      CampaignVariable<T> variable = new CampaignVariable<T>();
      variable.Campaign = campaign;
      variable.Name = name;
      variable.Player = player;
      variable.converter = VariableConverterService.GetCampaignConverter<T>();

      return variable;
    }

    /// <summary>
    /// The current value of this variable, otherwise the default value if unassigned (null or 0).
    /// </summary>
    public T Value
    {
      get => converter.GetCampaign(Campaign, Name, Player);
      set => converter.SetCampaign(Campaign, Name, value, Player);
    }

    /// <summary>
    /// Implicit conversion of the value of this variable.
    /// </summary>
    public static implicit operator T(CampaignVariable<T> value)
    {
      return value.Value;
    }

    /// <summary>
    /// Deletes the value of this variable.
    /// </summary>
    public void Delete() => converter.ClearCampaign(Campaign, Name, Player);

    public bool Equals(CampaignVariable<T> other)
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

      return Equals((CampaignVariable<T>) obj);
    }

    public override int GetHashCode()
    {
      return (Value != null ? Value.GetHashCode() : 0);
    }

    public static bool operator ==(CampaignVariable<T> left, CampaignVariable<T> right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(CampaignVariable<T> left, CampaignVariable<T> right)
    {
      return !Equals(left, right);
    }
  }
}