using System;
using NWN.Native.API;
using NWNX.NET.Native;

namespace Anvil.API
{
  public sealed class GffResource : IDisposable
  {
    private readonly CResGFF resGff;
    private readonly CResStruct rootStruct;

    internal unsafe GffResource(string name, CResGFF resGff)
    {
      this.resGff = resGff;
      NativeArray<byte>? fileType = this.resGff.m_pFileType;

      FileType = StringUtils.ReadFixedLengthString(fileType.Pointer, fileType.Length).Trim();

      rootStruct = new CResStruct();
      if (!resGff.GetTopLevelStruct(rootStruct).ToBool())
      {
        throw new ArgumentException($"Failed to initialize top level structure in gff resource {name}", nameof(resGff));
      }
    }

    ~GffResource()
    {
      Dispose(false);
    }

    /// <summary>
    /// Gets the file type/extension of this <see cref="GffResource"/>.
    /// </summary>
    public string FileType { get; }

    /// <summary>
    /// Gets the child <see cref="GffResourceField"/> at the specified index.
    /// </summary>
    public GffResourceField? this[int index] => GffResourceField.Create(resGff, rootStruct, (uint)index);

    /// <summary>
    /// Gets the child <see cref="GffResourceField"/> with the specified key.
    /// </summary>
    public GffResourceField? this[string index] => GffResourceField.Create(resGff, rootStruct, index);

    public void Dispose()
    {
      Dispose(true);
    }

    private void Dispose(bool disposing)
    {
      resGff.Dispose();
      rootStruct.Dispose();

      if (disposing)
      {
        GC.SuppressFinalize(this);
      }
    }
  }
}
