using System;

namespace Anvil.API
{
  public sealed class CassowaryException(string message) : Exception(message);
}
