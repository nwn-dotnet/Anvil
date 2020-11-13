using System;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public sealed class NWNXDamageEventAttribute : Attribute, IEventAttribute
  {
    void IEventAttribute.InitHook(string scriptName)
    {
      DamagePlugin.SetDamageEventScript(scriptName);
    }
  }
}
