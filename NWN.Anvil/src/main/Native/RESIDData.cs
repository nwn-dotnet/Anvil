using System.Runtime.InteropServices;

namespace Anvil.Native
{
  [StructLayout(LayoutKind.Sequential)]
  // ReSharper disable once InconsistentNaming
  public readonly struct RESIDData
  {
    public readonly uint m_resFileSource; // uint32_t
    public readonly uint m_resFileId; // uint32_t
    public readonly uint m_resTableId; // uint32_t
    public readonly uint m_resItemId; // uint32_t
  }
}
