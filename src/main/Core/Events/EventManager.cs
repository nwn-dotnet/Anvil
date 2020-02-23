using NLog;

namespace NWM.Core
{
  [Service]
  public sealed partial class EventManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
  }
}