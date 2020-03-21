using NLog;
using NWM.API;
using NWN;

namespace NWM.Core
{
  [Service]
  public sealed partial class EventManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private NwObject EventObjectSelf => NWScript.OBJECT_SELF.ToNwObject();
    private NwObject EnteringObject => NWScript.GetEnteringObject().ToNwObject();
    private NwObject ExitingObject => NWScript.GetExitingObject().ToNwObject();
  }
}