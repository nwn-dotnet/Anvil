using System.IO;
using Properties;

namespace Anvil.Services
{
  [ServiceBinding(typeof(HomeStorageResourceFactory))]
  internal sealed class HomeStorageResourceFactory
  {
    public HomeStorageResourceFactory()
    {
      WriteDefaultPaketResources();
      WriteDefaultNLogResources();
    }

    private void WriteDefaultPaketResources()
    {
      WriteIfNotExists(Path.Combine(HomeStorage.Paket, "paket.dependencies.orig"), HomeResources.PaketDefaultConfig);
    }

    private void WriteDefaultNLogResources()
    {
      WriteIfNotExists(HomeStorage.NLogConfig + ".orig", HomeResources.NLogDefaultConfig);
    }

    private void WriteIfNotExists(string path, string contents)
    {
      if (!File.Exists(path))
      {
        File.WriteAllText(path, contents);
      }
    }

    private void WriteIfNotExists(string path, byte[] contents)
    {
      if (!File.Exists(path))
      {
        File.WriteAllBytes(path, contents);
      }
    }


  }
}
