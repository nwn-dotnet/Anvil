using System;
using JetBrains.Annotations;
using NWM.API;
using NWM.API.Events;
using NWNX;
using EventHandler = NWM.Core.EventHandler;

namespace NWMX.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent<>))]
  public class NWNXEventAttribute : Attribute, IEventAttribute
  {
    public readonly string EventName;

    public NWNXEventAttribute(string eventName)
    {
      this.EventName = eventName;
    }

    public void InitHook(string scriptName)
    {
      EventsPlugin.SubscribeEvent(EventName, scriptName);
    }

    public void InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, string scriptName) where TObject : NwObject where TEvent : IEvent<TObject, TEvent>, new()
    {
      EventsPlugin.AddObjectToDispatchList(EventName, scriptName, nwObject);
    }
  }
}