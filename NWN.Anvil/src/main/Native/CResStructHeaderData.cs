using System.Runtime.InteropServices;

namespace Anvil.Native
{
  [StructLayout(LayoutKind.Sequential)]
  public readonly struct CResStructHeaderData
  {
    public readonly uint m_nType; // uint32_t
    public readonly uint m_nFields; // uint32_t
  }
}
