using System;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class GffResource
  {
    private readonly CResGFF resGff;
    private readonly CResStruct rootStruct;

    internal GffResource(string name, CResGFF resGff)
    {
      this.resGff = resGff;

      FileType = resGff.m_pFileType.ReadFixedLengthString();

      rootStruct = new CResStruct();
      if (!resGff.GetTopLevelStruct(rootStruct).ToBool())
      {
        throw new ArgumentException($"Failed to initialize top level structure in gff resource {name}", nameof(resGff));
      }
    }

    public string FileType { get; }

    public GffResourceField this[uint index]
    {
      get => GffResourceField.Create(resGff, rootStruct, index);
    }

    public GffResourceField this[string index]
    {
      get => GffResourceField.Create(resGff, rootStruct, index);
    }
  }
}
