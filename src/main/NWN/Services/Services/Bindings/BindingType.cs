namespace NWN.Services
{
  public enum BindingType
  {
    /// <summary>
    /// Creates and starts a single instance of the service during startup and uses the same instance when injected to other services.
    /// </summary>
    Singleton,
    /// <summary>
    /// Creates and starts a new instance of the service for each place the service is injected.
    /// </summary>
    Transient
  }
}
