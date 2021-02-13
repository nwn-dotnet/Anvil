namespace NWN.Services
{
  //! ## Examples
  //! @include PerformanceReportService.cs

  /// <summary>
  /// Implement this interface in your service to get a callback each server loop.
  /// </summary>
  public interface IUpdateable
  {
    /// <summary>
    /// Called once every main loop frame. You may safely use any of the NWN API in this callback, but it is recommended to
    /// keep the processing in this function to a minimum, as it may have adverse effect on server performance.
    /// </summary>
    void Update();
  }
}
