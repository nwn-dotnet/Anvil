/*
 * Define a method (OnScriptCalled) to be called when the NwScript "test_nwscript" would be called by the game.
 */

using Anvil.API;
using Anvil.Services;
using NLog;

// The "ServiceBinding" attribute indicates this class should be created on start, and available to other classes as a dependency "MyScriptHandler"
// You can also bind yourself to an interface or base class. The system also supports multiple bindings.
namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(BasicScriptHandler))]
  public class BasicScriptHandler
  {
    // Gets the server log. By default, this reports to "anvil.log"
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // This function will be called as if the same script was called by a toolset event, or by another script.
    // Script name must be <= 16 characters similar to the toolset.
    // This function must always return void, or a bool in the case of a conditional.
    // The NwObject parameter is optional, but if defined, must always be a single parameter of the NWObject type.
    [ScriptHandler("test_nwscript")]
    private void OnScriptCalled(CallInfo callInfo)
    {
      Log.Info($"test_nwscript called by {callInfo.ObjectSelf?.Name}");
    }
  }
}
