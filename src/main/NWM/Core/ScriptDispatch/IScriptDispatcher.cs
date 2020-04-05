namespace NWM.Core
{
  internal interface IScriptDispatcher
  {
    int ExecuteScript(string scriptName, uint oidSelf);
  }
}