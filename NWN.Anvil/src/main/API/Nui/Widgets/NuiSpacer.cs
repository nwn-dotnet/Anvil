namespace Anvil.API
{
  /// <summary>
  /// A special widget that just takes up layout space.<br/>
  /// Configure the space used with the Width and Height properties.
  /// </summary>
  public sealed class NuiSpacer : NuiWidget
  {
    public override string Type
    {
      get => "spacer";
    }
  }
}
