using System;
using Anvil.Internal;
using NWN.API;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  public static class IntegerExtensions
  {
    /// <summary>
    /// Reinterprets the specified value as a boolean.
    /// </summary>
    /// <param name="value">The value to reinterpret.</param>
    /// <returns>False if the value is 0, true for any non-0 value.</returns>
    public static bool ToBool(this int value)
    {
      return value != NWScript.FALSE;
    }

    /// <summary>
    /// Reinterprets the specified value as a integer.
    /// </summary>
    /// <param name="value">The value to reinterpret.</param>
    /// <returns>1 if true, 0 if false.</returns>
    public static int ToInt(this bool value)
    {
      return value ? NWScript.TRUE : NWScript.FALSE;
    }

    /// <summary>
    /// Converts the specified object ID value into a managed game object.
    /// </summary>
    /// <param name="objectId">The object ID to convert.</param>
    /// <typeparam name="T">The expected object type.</typeparam>
    /// <returns>The associated object if it exists and is of type T, otherwise null.</returns>
    public static T ToNwObjectSafe<T>(this uint objectId) where T : NwObject
    {
      return NwObject.CreateInternal(objectId) as T;
    }

    /// <summary>
    /// Converts the specified object ID value into a managed player object.
    /// </summary>
    /// <param name="objectId">The object ID to convert.</param>
    /// <param name="playerSearch">Methods to use to resolve the player.</param>
    /// <returns>The associated player for this object, otherwise null.</returns>
    public static unsafe NwPlayer ToNwPlayer(this uint objectId, PlayerSearch playerSearch = PlayerSearch.All)
    {
      CNWSPlayer player = null;
      if (playerSearch.HasFlag(PlayerSearch.Controlled))
      {
        player = LowLevel.ServerExoApp.GetClientObjectByObjectId(objectId);
      }

      if ((player == null || player.Pointer == IntPtr.Zero) && playerSearch.HasFlag(PlayerSearch.Login))
      {
        CExoLinkedListInternal players = LowLevel.ServerExoApp.m_pcExoAppInternal.m_pNWSPlayerList.m_pcExoLinkedListInternal;
        for (CExoLinkedListNode node = players.pHead; node != null; node = node.pNext)
        {
          CNWSPlayer current = CNWSPlayer.FromPointer(node.pObject);
          if (current.m_oidPCObject == objectId)
          {
            player = current;
            break;
          }
        }
      }

      return player != null && player.Pointer != IntPtr.Zero ? new NwPlayer(player) : null;
    }

    /// <summary>
    /// Converts the specified object ID value into a managed game object.
    /// </summary>
    /// <param name="objectId">The object ID to convert.</param>
    /// <typeparam name="T">The expected object type.</typeparam>
    /// <returns>The associated object if it exists, otherwise null.</returns>
    /// <exception cref="InvalidCastException">Thrown if the object associated with the object ID is not of type T.</exception>
    public static T ToNwObject<T>(this uint objectId) where T : NwObject
    {
      return (T)NwObject.CreateInternal(objectId);
    }

    /// <summary>
    /// Converts the specified object ID value into a managed game object.
    /// </summary>
    /// <param name="objectId">The object ID to convert.</param>
    /// <returns>The associated object if it exists, otherwise null.</returns>
    public static NwObject ToNwObject(this uint objectId)
    {
      return NwObject.CreateInternal(objectId);
    }

    /// <summary>
    /// Reinterprets the specified value as a signed byte.
    /// </summary>
    /// <param name="value">The value to reinterpret.</param>
    /// <returns>The reinterpreted value.</returns>
    public static sbyte AsSByte(this byte value)
    {
      return unchecked((sbyte)value);
    }

    /// <summary>
    /// Reinterprets the specified value as an unsigned byte.
    /// </summary>
    /// <param name="value">The value to reinterpret.</param>
    /// <returns>The reinterpreted value.</returns>
    public static byte AsByte(this sbyte value)
    {
      return unchecked((byte)value);
    }

    /// <summary>
    /// Reinterprets the specified value as a signed short.
    /// </summary>
    /// <param name="value">The value to reinterpret.</param>
    /// <returns>The reinterpreted value.</returns>
    public static short AsShort(this ushort value)
    {
      return unchecked((short)value);
    }

    /// <summary>
    /// Reinterprets the specified value as an unsigned short.
    /// </summary>
    /// <param name="value">The value to reinterpret.</param>
    /// <returns>The reinterpreted value.</returns>
    public static ushort AsUShort(this short value)
    {
      return unchecked((ushort)value);
    }
  }
}
