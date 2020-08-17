namespace NWN.Services
{
  internal interface IScriptDispatcher
  {
    ScriptHandleResult ExecuteScript(string scriptName, uint oidSelf);
  }
}