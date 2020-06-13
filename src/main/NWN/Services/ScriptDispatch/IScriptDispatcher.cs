namespace NWN.Services
{
  internal interface IScriptDispatcher
  {
    int ExecuteScript(string scriptName, uint oidSelf);
  }
}