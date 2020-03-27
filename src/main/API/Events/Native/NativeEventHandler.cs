using System;
using System.Reflection;

namespace NWM.API
{
  public abstract class NativeEventHandler<T> : EventHandler<T> where T : Enum
  {
    protected sealed override void RegisterDefaultScriptHandlers()
    {
      foreach (T value in Enum.GetValues(typeof(T)))
      {
        string scriptName = ScriptPrefix != null ? ScriptPrefix + GetScriptSuffix(value) : GetScriptSuffix(value);
        scriptToEventMap[scriptName] = value;
      }
    }

    private string GetScriptSuffix(Enum value)
    {
      FieldInfo enumVal = value.GetType().GetField(value.ToString());
      return enumVal.GetCustomAttribute<DefaultScriptSuffixAttribute>()?.ScriptSuffix ?? value.ToString().Substring(0, 4).ToLower();
    }
  }
}