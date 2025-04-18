using System.Runtime.InteropServices;
using Anvil.Services;
using NUnit.Framework;
using NWN.Native.API;

namespace Anvil.Tests.Services
{
  [TestFixture]
  public unsafe class HookServiceTests
  {
    [NativeFunction("_ZN11CAppManager26GetDungeonMasterEXERunningEv", "?GetDungeonMasterEXERunning@CAppManager@@QEAAHXZ")]
    private delegate int GetDungeonMasterExeRunning(void* pAppManager);

    [Inject]
    private static HookService HookService { get; set; } = null!;

    private static readonly delegate* unmanaged<void*, int> PHook1 = &OnHook1;
    private static readonly delegate* unmanaged<void*, int> PHook2 = &OnHook2;
    private static readonly delegate* unmanaged<void*, int> PHook3 = &OnHook3;

    private static FunctionHook<GetDungeonMasterExeRunning>? hook1;
    private static FunctionHook<GetDungeonMasterExeRunning>? hook2;
    private static FunctionHook<GetDungeonMasterExeRunning>? hook3;

    private static bool hook1Called;
    private static bool hook2Called;
    private static bool hook3Called;

    [SetUp]
    public void Setup()
    {
      hook1Called = false;
      hook2Called = false;
      hook3Called = false;

      hook1 = HookService.RequestHook<GetDungeonMasterExeRunning>(PHook1);
      hook2 = HookService.RequestHook<GetDungeonMasterExeRunning>(PHook2);
      hook3 = HookService.RequestHook<GetDungeonMasterExeRunning>(PHook3);
    }

    [TearDown]
    public void TearDown()
    {
      hook1?.Dispose();
      hook2?.Dispose();
      hook3?.Dispose();
    }

    [Test]
    public void HookOrderedDisposeTest()
    {
      hook1?.Dispose();
      hook1 = null;
      hook2?.Dispose();
      hook2 = null;

      int dmExeRunning = NWNXLib.AppManager().GetDungeonMasterEXERunning();

      Assert.That(dmExeRunning, Is.EqualTo(0));
      Assert.That(hook1Called, Is.False);
      Assert.That(hook2Called, Is.False);
      Assert.That(hook3Called, Is.True);
    }

    [Test]
    public void HookUnorderedDisposeTest()
    {
      hook2?.Dispose();
      hook2 = null;
      hook1?.Dispose();
      hook1 = null;

      int dmExeRunning = NWNXLib.AppManager().GetDungeonMasterEXERunning();

      Assert.That(dmExeRunning, Is.EqualTo(0));
      Assert.That(hook1Called, Is.False);
      Assert.That(hook2Called, Is.False);
      Assert.That(hook3Called, Is.True);
    }

    [UnmanagedCallersOnly]
    private static int OnHook1(void* pAppManager)
    {
      hook1Called = true;
      hook1?.CallOriginal(pAppManager);
      return 0;
    }

    [UnmanagedCallersOnly]
    private static int OnHook2(void* pAppManager)
    {
      hook2Called = true;
      hook2?.CallOriginal(pAppManager);
      return 0;
    }

    [UnmanagedCallersOnly]
    private static int OnHook3(void* pAppManager)
    {
      hook3Called = true;
      hook3?.CallOriginal(pAppManager);
      return 0;
    }
  }
}
