/*
 * Service binding basics - starts two services and logs a message for each.
 */

using Anvil.Services;
using NLog;

namespace NWN.Anvil.Samples
{
  // The "ServiceBinding" attribute indicates this class will be created on start, and available to other classes as "ServiceA".
  [ServiceBinding(typeof(ServiceA))]
  public class ServiceA
  {
    // Gets the logger for this service. By default, this reports to "logs.0/anvil.log"
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // As "ServiceA" has the ServiceBinding attribute, this constructor will be called during server startup.
    public ServiceA()
    {
      Log.Info("Service A Loaded!");
    }
  }

  // This class will be available to other classes as "ServiceB".
  [ServiceBinding(typeof(ServiceB))]
  public class ServiceB
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // As "ServiceB" also has the ServiceBinding attribute, this constructor will also be called during server startup,
    // but since "ServiceA" is specified as a parameter (dependency), it will only be started after "ServiceA" has loaded.
    public ServiceB(ServiceA serviceA)
    {
      Log.Info("Service B Loaded!");
    }
  }

  // Checking in the console, or "logs.0/anvil.log", the output should look like this:
  /*
[ServiceA] Service A Loaded!
[ServiceB] Service B Loaded!
*/
}
