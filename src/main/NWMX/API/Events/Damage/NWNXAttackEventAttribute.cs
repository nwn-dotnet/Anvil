using System;
using NWM.API;
using NWM.API.Events;
using NWNX;
using EventHandler = NWM.Core.EventHandler;

namespace NWMX.API.Events
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