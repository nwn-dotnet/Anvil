using System.Threading.Tasks;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests
{
  [TestFixture(Category = "TestRunner")]
  public sealed class TestRunnerTests
  {
    [Inject]
    private static VirtualMachine VirtualMachine { get; set; }

    [OneTimeSetUp]
    public void OneTimeSetupRunsInScriptContext()
    {
      Assert.IsTrue(VirtualMachine.IsInScriptContext);
    }

    [SetUp]
    public void SetupRunsInScriptContext()
    {
      Assert.IsTrue(VirtualMachine.IsInScriptContext);
    }

    [Test]
    public void TestRunsInScriptContext()
    {
      Assert.IsTrue(VirtualMachine.IsInScriptContext);
    }

    [Test]
    public async Task AsyncTestRunsInScriptContext()
    {
      Assert.IsTrue(VirtualMachine.IsInScriptContext);
      await NwTask.DelayFrame(1);
      Assert.IsTrue(VirtualMachine.IsInScriptContext);
    }

    [TearDown]
    public void TearDownRunsInScriptContext()
    {
      Assert.IsTrue(VirtualMachine.IsInScriptContext);
    }

    [OneTimeTearDown]
    public void OneTimeTearDownRunsInScriptContext()
    {
      Assert.IsTrue(VirtualMachine.IsInScriptContext);
    }
  }
}
