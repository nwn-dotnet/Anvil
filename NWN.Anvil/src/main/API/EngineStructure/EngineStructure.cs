using System;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// A pointer-type VM structure.
  /// </summary>
  public abstract class EngineStructure : IDisposable
  {
    private readonly IntPtr handle;

    private protected EngineStructure(IntPtr handle)
    {
      this.handle = handle;
    }

    ~EngineStructure()
    {
      ReleaseUnmanagedResources();
    }

    protected abstract int StructureId { get; }

    public static implicit operator IntPtr(EngineStructure engineStructure)
    {
      return engineStructure.handle;
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    private void ReleaseUnmanagedResources()
    {
      VM.FreeGameDefinedStructure(StructureId, handle);
    }
  }
}
