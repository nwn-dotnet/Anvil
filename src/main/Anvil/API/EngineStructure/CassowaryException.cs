using System;

namespace Anvil.API
{
  public sealed class CassowaryException : Exception
  {
    public CassowaryException(string message) : base(message) {}
  }
}
