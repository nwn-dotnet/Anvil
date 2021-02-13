/*
 * Store a C# DateTime object in a local variable by setting an epoch timestamp.
 */

using System;
using NWN.API;
using NWN.Core;

[LocalVariableConverter(typeof(DateTime))]
public class DateTimeLocalVariableConverter : ILocalVariableConverter<DateTime>
{
  public DateTime GetLocal(NwObject nwObject, string name)
  {
    return DateTime.UnixEpoch + TimeSpan.FromSeconds(NWScript.GetLocalInt(nwObject, name));
  }

  public void SetLocal(NwObject nwObject, string name, DateTime value)
  {
    int seconds = (int)(value.ToUniversalTime() - DateTime.UnixEpoch).TotalSeconds;
    NWScript.SetLocalInt(nwObject, name, seconds);
  }

  public void ClearLocal(NwObject nwObject, string name)
  {
    NWScript.DeleteLocalInt(nwObject, name);
  }
}
