using System;
using NWN.API.Events;
using NWN.Core.NWNX;
using EventHandler = NWN.Services.EventHandler;

namespace NWNX.API.Events
{
  public sealed class NWNXAttackEventAttribute : Attribute, IEventAttribute
  {
    void IEventAttribute.InitHook(string scriptName)
    {
      DamagePlugin.SetAttackEventScript(scriptName);
    }

    void IEventAttribute.InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, string scriptName)
    {
      throw new NotImplementedException();
    }
  }
}