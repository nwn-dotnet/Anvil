namespace Anvil.Services
{
  /// <summary>
  /// Interface that is invoked after all services have been initialised.
  /// </summary>
  public interface IInitializable
  {
    void Init();
  }
}
