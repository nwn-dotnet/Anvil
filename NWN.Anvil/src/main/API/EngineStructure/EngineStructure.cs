using System;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// A pointer-type VM structure.
  /// </summary>
  public abstract class EngineStructure : IDisposable
  {
    private IntPtr handle;
    private bool memoryOwn;

    private protected EngineStructure(IntPtr handle, bool memoryOwn)
    {
      this.handle = handle;
      this.memoryOwn = memoryOwn;

      if (memoryOwn)
      {
        GC.SuppressFinalize(this);
      }
    }

    ~EngineStructure()
    {
      ReleaseUnmanagedResources();
    }

    protected abstract int StructureId { get; }

    /// <summary>
    /// Gets if this object is valid.
    /// </summary>
    public bool IsValid => handle != IntPtr.Zero;

    public static implicit operator IntPtr(EngineStructure engineStructure)
    {
      if (!engineStructure.IsValid)
      {
        throw new InvalidOperationException("Engine structure is not valid.");
      }

      return engineStructure.handle;
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    private void ReleaseUnmanagedResources()
    {
      if (memoryOwn)
      {
        memoryOwn = false;
        VM.FreeGameDefinedStructure(StructureId, handle);
        handle = IntPtr.Zero;
      }
    }
  }
}
