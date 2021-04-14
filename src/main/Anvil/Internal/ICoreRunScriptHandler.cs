namespace Anvil.Internal
{
  internal interface ICoreRunScriptHandler
  {
    int OnRunScript(string script, uint oidSelf);
  }
}
