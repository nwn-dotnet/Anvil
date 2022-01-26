using System;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Async")]
  public sealed class NwTaskTests
  {
    [Inject]
    private static VirtualMachine VirtualMachine { get; set; }

    [Test(Description = "Starts an async task, then attempts to switch back to the main thread & script context.")]
    public async Task ReturnToMainThreadAfterSwitch()
    {
      await Task.Run(async () =>
      {
        await Task.Delay(TimeSpan.FromSeconds(0.1f));
      });

      Assert.That(VirtualMachine.IsInScriptContext, Is.False, "Expected to be outside of script context.");
      await NwTask.SwitchToMainThread();
      Assert.That(VirtualMachine.IsInScriptContext, Is.True, "Did not return to the main thread after SwitchToMainThread.");
    }
  }
}
