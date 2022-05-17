using System;
using NWN.Core;

namespace Anvil.API
{
  public sealed class LocalVariableCassowary : LocalVariable<Cassowary>
  {
    public override Cassowary? Value
    {
      get => NWScript.GetLocalCassowary(Object, Name);
      set => NWScript.SetLocalCassowary(Object, Name, value ?? throw new ArgumentNullException(nameof(value)));
    }

    public override void Delete()
    {
      NWScript.DeleteLocalCassowary(Object, Name);
    }
  }
}
