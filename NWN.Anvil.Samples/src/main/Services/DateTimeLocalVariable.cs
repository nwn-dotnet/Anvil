/*
 * Store a C# DateTime object in a local variable by setting an epoch timestamp.<br/>
 * Requires a reference to NWN.Core.
 */

using System;
using Anvil.API;
using NWN.Core;

namespace NWN.Anvil.Samples
{
  public class DateTimeLocalVariable : LocalVariable<DateTime>
  {
    public override DateTime Value
    {
      get => DateTime.UnixEpoch + TimeSpan.FromSeconds(NWScript.GetLocalInt(Object, Name));
      set
      {
        int seconds = (int)(value.ToUniversalTime() - DateTime.UnixEpoch).TotalSeconds;
        NWScript.SetLocalInt(Object, Name, seconds);
      }
    }

    public override void Delete()
    {
      NWScript.DeleteLocalInt(Object, Name);
    }
  }
}
