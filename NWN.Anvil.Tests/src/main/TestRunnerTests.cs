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
      Assert.That(VirtualMachine.IsInScriptContext, Is.True);
    }

    [SetUp]
    public void SetupRunsInScriptContext()
    {
      Assert.That(VirtualMachine.IsInScriptContext, Is.True);
    }

    [Test]
    public void TestRunsInScriptContext()
    {
      Assert.That(VirtualMachine.IsInScriptContext, Is.True);
    }

    [Test]
    public async Task AsyncTestRunsInScriptContext()
    {
      Assert.That(VirtualMachine.IsInScriptContext, Is.True);
      await NwTask.DelayFrame(1);
      Assert.That(VirtualMachine.IsInScriptContext, Is.True);
    }

    [TearDown]
    public void TearDownRunsInScriptContext()
    {
      Assert.That(VirtualMachine.IsInScriptContext, Is.True);
    }

    [OneTimeTearDown]
    public void OneTimeTearDownRunsInScriptContext()
    {
      Assert.That(VirtualMachine.IsInScriptContext, Is.True);
    }
  }
}
