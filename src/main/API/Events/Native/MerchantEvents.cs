namespace NWM.API
{
  public sealed class MerchantEvents : EventHandler<MerchantEventType>
  {
    protected override void HandleEvent(MerchantEventType eventType, NwObject objSelf)
    {
      switch (eventType)
      {
        case MerchantEventType.OpenStore:
          break;
        case MerchantEventType.StoreClosed:
          break;
      }
    }
  }
}