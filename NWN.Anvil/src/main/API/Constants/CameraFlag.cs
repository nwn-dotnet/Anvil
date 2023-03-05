using System;
using NWN.Core;

namespace Anvil.API
{
  [Flags]
  public enum CameraFlag
  {
    EnableCollision = NWScript.CAMERA_FLAG_ENABLE_COLLISION,
    DisableCollision = NWScript.CAMERA_FLAG_DISABLE_COLLISION,
    DisableShake = NWScript.CAMERA_FLAG_DISABLE_SHAKE,
    DisableScroll = NWScript.CAMERA_FLAG_DISABLE_SCROLL,
    DisableTurn = NWScript.CAMERA_FLAG_DISABLE_TURN,
    DisableTilt = NWScript.CAMERA_FLAG_DISABLE_TILT,
    DisableZoom = NWScript.CAMERA_FLAG_DISABLE_ZOOM,
  }
}
