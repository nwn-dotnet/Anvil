namespace Anvil.API
{
  /// <summary>
  /// Explicit interface to support the compiling of async/await.
  /// </summary>
  public interface IAwaitable
  {
    IAwaiter GetAwaiter();
  }

  /// <summary>
  /// Explicit interface to support the compiling of async/await.
  /// </summary>
  /// <typeparam name="TResult">The awaiter return value type.</typeparam>
  public interface IAwaitable<out TResult>
  {
    IAwaiter<TResult> GetAwaiter();
  }
}
