using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using NUnit.Framework;

namespace Anvil.Tests.Services
{
  [TestFixture]
  public sealed class ResourceManagerTests
  {
    [Inject]
    private static ResourceManager ResourceManager { get; set; } = null!;

    private readonly List<string> createdTempResources = [];

    [Test(Description = "A created temporary resource is available as a game resource.")]
    public void CreateTemporary2daResourceIsAvailable()
    {
      const string resourceContents = """
                                2DA V2.0

                                    LABEL    TEST
                                """;

      const string baseResourceName = "testres";
      const string resourceName = baseResourceName + ".2da";

      ResourceManager.WriteTempResource(resourceName, resourceContents);
      createdTempResources.Add(resourceName);

      Assert.That(ResourceManager.IsValidResource(baseResourceName, ResRefType.TWODA), Is.True);
    }

    [Test(Description = "A non-existing resource is considered invalid.")]
    [TestCase(ResRefType.ARE)]
    [TestCase(ResRefType.BIC)]
    [TestCase(ResRefType.NSS)]
    public void InvalidResourceIsUnavailable(ResRefType resRefType)
    {
      Assert.That(ResourceManager.IsValidResource("invalid_res", resRefType), Is.False);
    }

    [TearDown]
    public void CleanupCreatedResources()
    {
      foreach (string resource in createdTempResources)
      {
        ResourceManager.DeleteTempResource(resource);
      }

      createdTempResources.Clear();
    }
  }
}
