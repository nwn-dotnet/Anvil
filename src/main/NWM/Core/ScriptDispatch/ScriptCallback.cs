using System;
using System.Reflection;
using NLog;
using NWM.API;

namespace NWM.Core
{
  internal class ScriptCallback
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private Action scriptHandler;
    private Func<bool> conditionalHandler;
    private Action<NwObject> nwObjectScriptHandler;
    private Func<NwObject, bool> nwObjectConditionalHandler;

    public int ProcessCallbacks(uint objSelfId)
    {
      int result = ScriptDispatchConstants.SCRIPT_NOT_HANDLED;
      NwObject objSelf = null;

      if (scriptHandler != null)
      {
        scriptHandler();
        result = ScriptDispatchConstants.SCRIPT_HANDLED;
      }
      else if (nwObjectScriptHandler != null)
      {
        objSelf = objSelfId.ToNwObject();
        nwObjectScriptHandler(objSelf);
        result = ScriptDispatchConstants.SCRIPT_HANDLED;
      }

      if (conditionalHandler != null)
      {
        result = conditionalHandler().ToInt();
      }
      else if (nwObjectConditionalHandler != null)
      {
        objSelf = objSelf != null ? objSelf : objSelfId.ToNwObject();
        result = nwObjectConditionalHandler(objSelf).ToInt();
      }

      return result;
    }

    public void AddCallback(object service, MethodInfo method, string scriptName)
    {
      switch (GetMethodType(method))
      {
        case MethodType.Handler:
          if (scriptHandler != null || nwObjectScriptHandler != null)
          {
            Log.Warn($"Script Handler {scriptName} is already registered by: \"{method.GetFullName()}\"");
            return;
          }

          scriptHandler = (Action) Delegate.CreateDelegate(typeof(Action), service, method);
          break;
        case MethodType.ObjectHandler:
          if (scriptHandler != null || nwObjectScriptHandler != null)
          {
            Log.Warn($"Script Handler {scriptName} is already registered by: \"{method.GetFullName()}\"");
            return;
          }

          nwObjectScriptHandler = (Action<NwObject>) Delegate.CreateDelegate(typeof(Action<NwObject>), service, method);
          break;
        case MethodType.Conditional:
          if (conditionalHandler != null || nwObjectConditionalHandler != null)
          {
            Log.Warn($"Conditional Handler {scriptName} is already registered by: \"{method.GetFullName()}\"");
            return;
          }

          conditionalHandler = (Func<bool>) Delegate.CreateDelegate(typeof(Func<bool>), service, method);
          break;
        case MethodType.ObjectConditional:
          if (conditionalHandler != null || nwObjectConditionalHandler != null)
          {
            Log.Warn($"Conditional Handler {scriptName} is already registered by: \"{method.GetFullName()}\"");
            return;
          }

          nwObjectConditionalHandler = (Func<NwObject, bool>) Delegate.CreateDelegate(typeof(Func<NwObject, bool>), service, method);
          break;
        case MethodType.Invalid:
          Log.Error($"Script Handler has invalid parameters or return value: {scriptName} -> {method.GetFullName()}");
          return;
      }

      Log.Info($"Registered Script Handler: {scriptName} -> {method.GetFullName()}");
    }

    private MethodType GetMethodType(MethodInfo method)
    {
      ParameterInfo[] parameters = method.GetParameters();
      if (parameters.Length == 0)
      {
        return method.ReturnType == typeof(bool) ? MethodType.Conditional : MethodType.Handler;
      }

      if (parameters.Length == 1 && parameters[0].ParameterType == typeof(NwObject))
      {
        return method.ReturnType == typeof(bool) ? MethodType.ObjectConditional : MethodType.ObjectHandler;
      }

      return MethodType.Invalid;
    }

    private enum MethodType
    {
      Invalid = 0,
      Handler,
      ObjectHandler,
      Conditional,
      ObjectConditional,
    }
  }
}