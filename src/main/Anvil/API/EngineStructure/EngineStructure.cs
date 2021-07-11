using System;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// Represents a pointer-type VM structure.
  /// </summary>
  public abstract class EngineStructure : IDisposable
  {
    private readonly IntPtr handle;

    protected abstract int StructureId { get; }

    private protected EngineStructure(IntPtr handle)
    {
      this.handle = handle;
    }

    private void ReleaseUnmanagedResources()
    {
      VM.FreeGameDefinedStructure(StructureId, handle);
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    ~EngineStructure()
    {
      ReleaseUnmanagedResources();
    }

    public static implicit operator IntPtr(EngineStructure engineStructure)
    {
      return engineStructure.handle;
    }
  }
}
