namespace Anvil.Services
{
  /// <summary>
  /// Interface that is invoked after the server has been shutdown, and during reloads.
  /// </summary>
  /// <remarks>
  /// This interface should only be used to release function hooks, or to replicate data that was written during server shutdown (e.g. characters).<br/>
  /// When this is called, the server instance has already been destroyed, and you cannot access any APIs.<br/>
  /// Implement <see cref="System.IDisposable"/> instead to receive a callback just before server shutdown.
  /// </remarks>
  public interface ILateDisposable
  {
    void LateDispose();
  }
}
