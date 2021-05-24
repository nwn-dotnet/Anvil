using System;
using Anvil.Internal;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  public static class IntegerExtensions
  {
    public static bool ToBool(this int value)
    {
      return value == NWScript.TRUE;
    }

    public static int ToInt(this bool value)
    {
      return value ? NWScript.TRUE : NWScript.FALSE;
    }

    public static T ToNwObjectSafe<T>(this uint objectId) where T : NwObject
    {
      return NwObject.CreateInternal(objectId) as T;
    }

    public static unsafe NwPlayer ToNwPlayer(this uint objectId, bool isControlledCreature = true)
    {
      if (isControlledCreature)
      {
        CNWSPlayer player = LowLevel.ServerExoApp.GetClientObjectByObjectId(objectId);
        return player != null && player != IntPtr.Zero ? new NwPlayer(player) : null;
      }

      CExoLinkedListInternal players = LowLevel.ServerExoApp.m_pcExoAppInternal.m_pNWSPlayerList.m_pcExoLinkedListInternal;
      for (CExoLinkedListNode node = players.pHead; node != null; node = node.pNext)
      {
        CNWSPlayer player = new CNWSPlayer(node.pObject, false);
        if (player.m_oidPCObject == objectId)
        {
          return player.ToNwPlayer();
        }
      }

      return null;
    }

    public static Lazy<NwObject> ToNwObjectLazy(this uint objectId)
    {
      return new Lazy<NwObject>(() => ToNwObject(objectId));
    }

    public static T ToNwObject<T>(this uint objectId) where T : NwObject
    {
      return (T) NwObject.CreateInternal(objectId);
    }

    public static NwObject ToNwObject(this uint objectId)
    {
      return NwObject.CreateInternal(objectId);
    }
  }
}
