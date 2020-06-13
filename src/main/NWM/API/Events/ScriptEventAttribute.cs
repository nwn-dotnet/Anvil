using System;
using JetBrains.Annotations;
using NLog;
using NWM.API.Constants;
using NWM.Core;
using NWN.Core;
using EventHandler = NWM.Core.EventHandler;

namespace NWM.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent<>))]
  public class ScriptEventAttribute : Attribute, IEventAttribute
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    public EventScriptType EventScriptType;

    public ScriptEventAttribute(EventScriptType eventScriptType)
    {
      this.EventScriptType = eventScriptType;
    }

    // No initial subscribe for script events.
    public void InitHook(string scriptName) {}

    public void InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, string scriptName) where TEvent : IEvent<TObject, TEvent>, new() where TObject : NwObject
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