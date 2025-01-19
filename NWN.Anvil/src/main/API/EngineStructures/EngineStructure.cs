using System;
using NWNX.NET;

namespace Anvil.API
{
  /// <summary>
  /// A pointer-type VM structure.
  /// </summary>
  public abstract class EngineStructure : IDisposable
  {
    private IntPtr handle;
    private bool memoryOwn;

    /// <summary>
    /// Gets if this object is valid.
    /// </summary>
    public bool IsValid => handle != IntPtr.Zero;

    protected abstract int StructureId { get; }

    private protected EngineStructure(IntPtr handle, bool memoryOwn)
    {
      this.handle = handle;
      this.memoryOwn = memoryOwn;
    }

    ~EngineStructure()
    {
      ReleaseUnmanagedResources();
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
        NWNXAPI.FreeGameDefinedStructure(StructureId, handle);
        handle = IntPtr.Zero;
      }
    }

    public static implicit operator IntPtr(EngineStructure engineStructure)
    {
      if (engineStructure == null || !engineStructure.IsValid)
      {
        throw new InvalidOperationException("Engine structure is not valid.");
      }

      return engineStructure.handle;
    }
  }
}
