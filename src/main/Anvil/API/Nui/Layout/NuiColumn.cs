namespace Anvil.API
{
  /// <summary>
  /// A layout element that will auto-space all elements inside of it and advise the parent about its desired size.
  /// </summary>
  public sealed class NuiColumn : NuiLayout
  {
    public override string Type { get => "col"; }
  }
}
