using System.Collections.Generic;
using System.IO;

namespace Anvil.Plugins
{
  internal sealed class PaketAssemblyLoadFile
  {
    private const string LoadPrefix = "#r ";

    private readonly string rootPath;

    internal Dictionary<string, string> AssemblyPaths { get; } = new Dictionary<string, string>();

    public PaketAssemblyLoadFile(string path)
    {
      rootPath = Path.GetFullPath(Path.GetDirectoryName(path)!);

      using StreamReader streamReader = File.OpenText(path);
      while (!streamReader.EndOfStream)
      {
        string line = streamReader.ReadLine();
        if (line!.StartsWith("#r"))
        {
          ProcessLine(line);
        }
      }
    }

    private void ProcessLine(string line)
    {
      string relativeAssemblyPath = line[LoadPrefix.Length..].Trim().Trim('"');
      string fullAssemblyPath = Path.GetFullPath(relativeAssemblyPath, rootPath);

      AssemblyPaths[Path.GetFileNameWithoutExtension(fullAssemblyPath)] = fullAssemblyPath;
    }
  }
}
