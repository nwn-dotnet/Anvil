using System;
using JetBrains.Annotations;
using NWN.API.Events;
using NWN.Core.NWNX;
using EventHandler = NWN.Services.EventHandler;

namespace NWNX.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(Event<>))]
  public class NWNXEventAttribute : Attribute, IEventAttribute
  {
    public readonly string EventName;

    public NWNXEventAttribute(string eventName)
    {
      this.EventName = eventName;
    }

    void IEventAttribute.InitHook(string scriptName)
    {
      EventsPlugin.SubscribeEvent(EventName, scriptName);
    }

    void IEventAttribute.InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, string scriptName)
    {
      EventsPlugin.AddObjectToDispatchList(EventName, scriptName, nwObject);
    }
  }
}
