using System;

namespace Anvil.API
{
  /// <summary>
  /// Exception thrown when a Cassowary solver operation fails.
  /// </summary>
  /// <param name="message">The error message describing the failure.</param>
  public sealed class CassowaryException(string message) : Exception(message);
}
