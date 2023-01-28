using System;
using System.Text;
using Anvil.Internal;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.Services
{
  /// <summary>
  /// Manages string conversion for NWN.Core, NWN.Native and the StringHelper utility class.
  /// </summary>
  public sealed class EncodingService : ICoreService
  {
    /// <summary>
    /// Gets or sets the encoding used by anvil to convert strings.
    /// </summary>
    public Encoding Encoding
    {
      get => NWNCore.Encoding;
      set
      {
        if (value == null)
        {
          throw new NullReferenceException("Encoding must not be null.");
        }

        NWNCore.Encoding = value;
        StringHelper.Encoding = value;
      }
    }

    void ICoreService.Init()
    {
      Encoding = Encoding.GetEncoding(EnvironmentConfig.Encoding);
    }

    void ICoreService.Load() {}

    void ICoreService.Shutdown() {}

    void ICoreService.Start() {}

    void ICoreService.Unload() {}
  }
}
