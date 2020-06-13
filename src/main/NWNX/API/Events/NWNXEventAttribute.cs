using System;
using JetBrains.Annotations;
using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;
using EventHandler = NWN.Services.EventHandler;

namespace NWNX.API.Events
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