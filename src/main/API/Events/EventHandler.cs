using System;
using System.Collections.Generic;
using NWN;

namespace NWM.API
{
  public abstract class EventHandler
  {
    internal string ScriptPrefix { get; private set; }
    internal abstract bool ProcessScriptEvent(string scriptName, NwObject objSelf);
    protected abstract void RegisterDefaultScriptHandlers();

    internal void Init(string scriptPrefix)
    {
      this.ScriptPrefix = scriptPrefix;
      RegisterDefaultScriptHandlers();
    }
  }

  public abstract class EventHandler<T> : EventHandler where T : Enum
  {
    protected NwGameObject EnteringObject => NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
    protected NwGameObject ExitingObject => NWScript.GetExitingObject().ToNwObject<NwGameObject>();

    private Dictionary<string, T> scriptToEventMap = new Dictionary<string, T>();

    protected abstract void HandleEvent(T eventType, NwObject objSelf);

    protected sealed override void RegisterDefaultScriptHandlers()
    {
      foreach (T value in Enum.GetValues(typeof(T)))
      {
        scriptToEventMap[ScriptPrefix + value.GetDefaultScriptSuffix()] = value;
      }
    }

    public void SetScriptHandler(T eventType, string scriptName)
    {
      scriptToEventMap[scriptName] = eventType;
    }

    internal void Init(Dictionary<string, T> handlerMap)
    {
      this.scriptToEventMap = new Dictionary<string, T>(handlerMap);
    }

    internal override bool ProcessScriptEvent(string scriptName, NwObject objSelf)
    {
      if (!scriptToEventMap.TryGetValue(scriptName, out T eventType))
      {
        return false;
      }

      HandleEvent(eventType, objSelf);
      return true;
    }
  }
}