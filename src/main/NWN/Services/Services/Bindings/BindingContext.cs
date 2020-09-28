namespace NWN.Services
{
  internal enum BindingContext
  {
    /// <summary>
    /// Indicates this service can be used by other services only.
    /// </summary>
    Service = 0,

    /// <summary>
    /// Indicates this service can be used by other services, and the managed API.
    /// </summary>
    API = 1
  }
}
