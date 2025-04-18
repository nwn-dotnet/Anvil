using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.Services.API.Utils
{
  [TestFixture]
  public sealed class VirtualMachineTests
  {
    [Inject]
    private static VirtualMachine VirtualMachine { get; set; } = null!;

    [Inject]
    private static ScriptHandleFactory ScriptHandleFactory { get; set; } = null!;

    [Test(Description = "Running ExecuteScript correctly executes the specified script with the right object context.")]
    [Timeout(5000)]
    public void ExecuteScriptAssignsCorrectObjectSelf()
    {
      foreach (NwObject nwObject in GetTestObjects())
      {
        ExecuteScript(nwObject);
      }
    }

    [Test(Description = "Running ExecuteScript in the main loop correctly executes the specified script with the right object context.")]
    [Timeout(5000)]
    public async Task ExecuteScriptInvalidContextAssignsCorrectObjectSelf()
    {
      await NwTask.NextFrame();
      foreach (NwObject nwObject in GetTestObjects())
      {
        ExecuteScript(nwObject);
      }
    }

    private void ExecuteScript(NwObject nwObject)
    {
      const string scriptParamKey = "someParamKey";
      const string scriptParamValue = "someParamValue";

      ScriptCallbackHandle handle = ScriptHandleFactory.CreateUniqueHandler(info =>
      {
        Assert.That(info.ObjectSelf, Is.EqualTo(nwObject));
        Assert.That(info.ScriptParams[scriptParamKey], Is.EqualTo(scriptParamValue));
        return ScriptHandleResult.Handled;
      });

      VirtualMachine.Execute(handle.ScriptName, nwObject, (scriptParamKey, scriptParamValue));
    }

    private static IEnumerable<NwObject> GetTestObjects()
    {
      yield return NwModule.Instance;

      foreach (NwGameObject gameObject in NwModule.Instance.Areas.First().Objects)
      {
        yield return gameObject;
      }
    }
  }
}
