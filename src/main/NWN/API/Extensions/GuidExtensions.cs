using System;

namespace NWN.API
{
  /// <summary>
  /// GUID/UUID extension methods for resolving game objects, and GUID serialisation.
  /// </summary>
  public static class GuidExtensions
  {
    /// <summary>
    /// Attempts to resolve a living active game object from the specified UUID.
    /// </summary>
    /// <param name="objectId">The UUID of the object.</param>
    /// <typeparam name="T">The expected object type. If the object is not this type.</typeparam>
    /// <returns>The game object with the given UUID and the specified type, otherwise returns null.</returns>
    public static T ToNwObjectSafe<T>(this Guid objectId) where T : NwObject
    {
      return NwObjectFactory.CreateInternal(objectId) as T;
    }

    /// <summary>
    /// Attempts to resolve a living active game object from the specified UUID.
    /// </summary>
    /// <param name="objectId">The UUID of the object.</param>
    /// <typeparam name="T">The expected object type.</typeparam>
    /// <returns>The game object with the given UUID, otherwise returns null if it does not exist.</returns>
    /// <exception cref="InvalidCastException">Object is not type T. See <see cref="ToNwObjectSafe{T}"/> if null should be returned in this case.</exception>
    public static T ToNwObject<T>(this Guid objectId) where T : NwObject
    {
      return (T) NwObjectFactory.CreateInternal(objectId);
    }

    /// <summary>
    /// Attempts to resolve a living active game object from the specified UUID.
    /// </summary>
    /// <param name="objectId">The UUID of the object.</param>
    /// <returns>The game object with the given UUID, otherwise returns null if it does not exist.</returns>
    public static NwObject ToNwObject(this Guid objectId)
    {
      return NwObjectFactory.CreateInternal(objectId);
    }

    /// <summary>
    /// Converts this GUID instance to a native compatible UUID string.
    /// </summary>
    /// <param name="guid">The GUID instance to convert.</param>
    /// <returns>The hyphenated UUID string, matching NWN's formatting.</returns>
    public static string ToUUIDString(this Guid guid)
    {
      return guid.ToString("D");
    }
  }
}