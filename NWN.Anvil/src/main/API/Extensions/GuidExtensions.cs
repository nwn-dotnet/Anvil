using System;

namespace Anvil.API
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
    /// <typeparam name="T">The expected object type.</typeparam>
    /// <returns>The game object with the given UUID, otherwise returns null if it does not exist.</returns>
    /// <exception cref="InvalidCastException">Thrown if the resolved object is not of type <typeparamref name="T"/>.
    /// Use <see cref="ToNwObjectSafe{T}"/> to return null instead of throwing.</exception>
    public static T? ToNwObject<T>(this Guid objectId) where T : NwObject
    {
      return (T?)NwObject.CreateInternal(objectId);
    }

    /// <summary>
    /// Attempts to resolve a living active game object from the specified UUID.
    /// </summary>
    /// <param name="objectId">The UUID of the object.</param>
    /// <returns>The game object with the given UUID, otherwise returns null if it does not exist.</returns>
    public static NwObject? ToNwObject(this Guid objectId)
    {
      return NwObject.CreateInternal(objectId);
    }

    /// <summary>
    /// Attempts to resolve a living active game object from the specified UUID.
    /// </summary>
    /// <param name="objectId">The UUID of the object.</param>
    /// <typeparam name="T">The expected object type.</typeparam>
    /// <returns>The game object with the given UUID and the specified type, otherwise returns null if the resolved object is not of type <typeparamref name="T"/>.</returns>
    public static T? ToNwObjectSafe<T>(this Guid objectId) where T : NwObject
    {
      return NwObject.CreateInternal(objectId) as T;
    }

    /// <summary>
    /// Converts this GUID instance to a native compatible UUID string.
    /// </summary>
    /// <param name="guid">The GUID instance to convert.</param>
    /// <returns>The hyphenated UUID string (format "D"), matching NWN's UUID formatting.</returns>
    public static string ToUUIDString(this Guid guid)
    {
      return guid.ToString("D");
    }
  }
}
