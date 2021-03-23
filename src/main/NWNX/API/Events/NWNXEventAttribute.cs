using System;
using JetBrains.Annotations;
using NWN.API.Events;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent))]
  public sealed class NWNXEventAttribute : Attribute, IEventAttribute
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
  }
}
