using System;
using System.Reflection;
using Anvil.API;
using NLog;
using Action = System.Action;

namespace Anvil.Services
{
  internal sealed class ScriptCallback
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly string scriptName;

    private Func<bool>? conditionalHandler;
    private Func<CallInfo, bool>? conditionalWithMetaHandler;

    private Action? scriptHandler;
    private Action<CallInfo>? scriptHandlerWithMetaHandler;

    public ScriptCallback(string scriptName)
    {
      this.scriptName = scriptName;
    }

    private enum MethodType
    {
      Invalid = 0,
      Handler,
      HandlerWithMeta,
      Conditional,
      ConditionalWithMeta,
    }

    public void AddCallback(object service, MethodInfo method)
    {
      switch (GetMethodType(method))
      {
        case MethodType.Handler:
          if (scriptHandler != null || scriptHandlerWithMetaHandler != null)
          {
            Log.Warn("Script Handler {ScriptName} is already registered by {MethodName}", scriptName, method.GetFullName());
            return;
          }

          scriptHandler = (Action)Delegate.CreateDelegate(typeof(Action), service, method);
          break;
        case MethodType.HandlerWithMeta:
          if (scriptHandler != null || scriptHandlerWithMetaHandler != null)
          {
            Log.Warn("Script Handler {ScriptName} is already registered by {MethodName}", scriptName, method.GetFullName());
            return;
          }

          scriptHandlerWithMetaHandler = (Action<CallInfo>)Delegate.CreateDelegate(typeof(Action<CallInfo>), service, method);
          break;
        case MethodType.Conditional:
          if (conditionalHandler != null || conditionalWithMetaHandler != null)
          {
            Log.Warn("Conditional Handler {ScriptName} is already registered by {MethodName}", scriptName, method.GetFullName());
            return;
          }

          conditionalHandler = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), service, method);
          break;
        case MethodType.ConditionalWithMeta:
          if (conditionalHandler != null || conditionalWithMetaHandler != null)
          {
            Log.Warn("Conditional Handler {ScriptName} is already registered by {MethodName}", scriptName, method.GetFullName());
            return;
          }

          conditionalWithMetaHandler = (Func<CallInfo, bool>)Delegate.CreateDelegate(typeof(Func<CallInfo, bool>), service, method);
          break;
        case MethodType.Invalid:
          Log.Error("Script Handler has invalid parameters or return value: {ScriptName} -> {MethodName}", scriptName, method.GetFullName());
          return;
      }

      Log.Info("Registered Script Handler: {ScriptName} -> {MethodName}", scriptName, method.GetFullName());
    }

    public ScriptHandleResult ProcessCallbacks(uint objSelfId)
    {
      ScriptHandleResult result = ScriptHandleResult.NotHandled;
      NwObject? objSelf = null;

      if (scriptHandler != null)
      {
        scriptHandler();
        result = ScriptHandleResult.Handled;
      }
      else if (scriptHandlerWithMetaHandler != null)
      {
        objSelf = objSelfId.ToNwObject();
        CallInfo meta = new CallInfo(scriptName, objSelf);
        scriptHandlerWithMetaHandler(meta);
        result = ScriptHandleResult.Handled;
      }

      if (conditionalHandler != null)
      {
        result = conditionalHandler() ? ScriptHandleResult.True : ScriptHandleResult.False;
      }
      else if (conditionalWithMetaHandler != null)
      {
        objSelf ??= objSelfId.ToNwObject();
        CallInfo meta = new CallInfo(scriptName, objSelf);
        result = conditionalWithMetaHandler(meta) ? ScriptHandleResult.True : ScriptHandleResult.False;
      }

      return result;
    }

    private MethodType GetMethodType(MethodInfo method)
    {
      ParameterInfo[] parameters = method.GetParameters();
      if (parameters.Length == 0)
      {
        return method.ReturnType == typeof(bool) ? MethodType.Conditional : MethodType.Handler;
      }

      if (parameters.Length == 1 && parameters[0].ParameterType == typeof(CallInfo))
      {
        return method.ReturnType == typeof(bool) ? MethodType.ConditionalWithMeta : MethodType.HandlerWithMeta;
      }

      return MethodType.Invalid;
    }
  }
}
