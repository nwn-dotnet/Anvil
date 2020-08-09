using System;
using JetBrains.Annotations;
using NLog;
using NWN.API.Constants;
using NWN.Core;
using NWN.Services;
using EventHandler = NWN.Services.EventHandler;

namespace NWN.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(Event<>))]
  internal class ScriptEventAttribute : Attribute, IEventAttribute
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// The native event type for this event.
    /// </summary>
    public readonly EventScriptType EventScriptType;

    public ScriptEventAttribute(EventScriptType eventScriptType)
    {
      this.EventScriptType = eventScriptType;
    }

    // No initial subscribe for script events.
    void IEventAttribute.InitHook(string scriptName) {}

    void IEventAttribute.InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, string scriptName)
    {
      string existingScript = NWScript.GetEventScript(nwObject, (int) EventScriptType);
      if (existingScript == eventHandler.ScriptName)
      {
        return;
      }

      Log.Debug($"Hooking native script event \"{EventScriptType}\" on object \"{nwObject.Name}\". Previous script: \"{existingScript}\"");

      NWScript.SetEventScript(nwObject, (int) EventScriptType, eventHandler.ScriptName);
      if (!string.IsNullOrEmpty(existingScript))
      {
        eventHandler.Subscribe<TObject, TEvent>(nwObject, gameEvent => ContinueWithNative(existingScript, nwObject));
      }
    }

    private void ContinueWithNative(string scriptName, NwObject objSelf)
    {
      Interop.ExecuteNss(scriptName, objSelf);
    }
  }
}