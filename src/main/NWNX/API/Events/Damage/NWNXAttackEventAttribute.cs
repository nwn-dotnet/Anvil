using System;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public sealed class NWNXAttackEventAttribute : Attribute, IEventAttribute
  {
    void IEventAttribute.InitHook(string scriptName)
    {
      DamagePlugin.SetAttackEventScript(scriptName);
    }
  }
}
