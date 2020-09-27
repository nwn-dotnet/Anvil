namespace NWN.API
{
  public interface ICampaignVariableConverter {}

  public interface ICampaignVariableConverter<T> : ICampaignVariableConverter
  {
    T GetCampaign(string campaign, string name, NwPlayer player);

    void SetCampaign(string campaign, string name, T value, NwPlayer player);

    void ClearCampaign(string campaign, string name, NwPlayer player);
  }
}
