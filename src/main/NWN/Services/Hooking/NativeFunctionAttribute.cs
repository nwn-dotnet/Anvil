using System;

namespace NWN.Services
{
  /// <summary>
  /// Indicates that this delegate represents a native game function.
  /// </summary>
  [AttributeUsage(AttributeTargets.Delegate)]
  public class NativeFunctionAttribute : Attribute
  {
    public uint Address { get; }

    /// <summary>
    /// Defines this delegate as a native function representation.
    /// </summary>
    /// <param name="address">The address of the native function. Use the constants available in the NWN.Native.NWNXLib.Functions library.</param>
    public NativeFunctionAttribute(uint address)
    {
      Address = address;
    }
  }
}
