using System;
using NWM.API;
using NWM.API.Events;
using NWN.Core.NWNX;
using EventHandler = NWM.Core.EventHandler;

namespace NWMX.API.Events
{
  public sealed class NWNXDamageEventAttribute : Attribute, IEventAttribute
  {
    public void InitHook(string scriptName)
    {
      DamagePlugin.SetDamageEventScript(scriptName);
    }

    public void InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, string scriptName) where TObject : NwObject where TEvent : IEvent<TObject, TEvent>, new()
    {
      throw new NotImplementedException();
    }
  }
}