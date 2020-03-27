namespace NWM.API
{
  public enum MerchantEventType
  {
    OpenStore,
    StoreClosed
  }

  public sealed class MerchantEvents : NativeEventHandler<MerchantEventType>
  {
    protected override void HandleEvent(MerchantEventType eventType, NwObject objSelf)
    {
      throw new System.NotImplementedException();
    }
  }
}