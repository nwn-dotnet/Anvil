using System;
using NWN.Services;

namespace NWN.API
{
  public abstract class CampaignVariable
  {
    private protected static VariableConverterService VariableConverterService { get; private set; }

    [ServiceBinding(typeof(APIBindings))]
    [BindingOrder(BindingOrder.API)]
    internal sealed class APIBindings
    {
      public APIBindings(VariableConverterService variableConverterService)
      {
        VariableConverterService = variableConverterService;
      }
    }

    public string Campaign { get; protected set; }

    public string Name { get; protected set; }

    public NwPlayer Player { get; protected set; }

    /// <summary>
    /// Deletes the value of this variable.
    /// </summary>
    public abstract void Delete();
  }

  public sealed class CampaignVariable<T> : CampaignVariable, IEquatable<CampaignVariable<T>>
  {
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
    /// Gets or sets the current value of this variable. Returns the default value of T if unassigned (null or 0).
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

    public override void Delete() => converter.ClearCampaign(Campaign, Name, Player);

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
      return Value != null ? Value.GetHashCode() : 0;
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
