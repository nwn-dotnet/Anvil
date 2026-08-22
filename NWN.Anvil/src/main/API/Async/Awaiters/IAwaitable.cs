namespace Anvil.API
{
  /// <summary>
  /// Explicit interface to support the compiling of async/await.
  /// </summary>
  public interface IAwaitable
  {
    /// <summary>
    /// Gets the awaiter instance used by the C# async/await pattern.
    /// </summary>
    /// <returns>An awaiter that schedules and completes the asynchronous operation.</returns>
    IAwaiter GetAwaiter();
  }

  /// <summary>
  /// Explicit interface to support the compiling of async/await.
  /// </summary>
  /// <typeparam name="TResult">The awaiter return value type.</typeparam>
  public interface IAwaitable<out TResult>
  {
    /// <summary>
    /// Gets the awaiter for this asynchronous operation producing a result.
    /// </summary>
    /// <returns>An awaiter that yields a <typeparamref name="TResult"/> on completion.</returns>
    IAwaiter<TResult> GetAwaiter();
  }
}
