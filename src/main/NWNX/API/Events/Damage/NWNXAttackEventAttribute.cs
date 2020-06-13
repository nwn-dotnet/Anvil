using System;
using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;
using EventHandler = NWN.Services.EventHandler;

namespace NWNX.API.Events
{
  public sealed class NWNXAttackEventAttribute : Attribute, IEventAttribute
  {
    public void InitHook(string scriptName)
    {
      DamagePlugin.SetAttackEventScript(scriptName);
    }

    public void InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, string scriptName) where TObject : NwObject where TEvent : IEvent<TObject, TEvent>, new()
    {
      throw new NotImplementedException();
    }
  }
}