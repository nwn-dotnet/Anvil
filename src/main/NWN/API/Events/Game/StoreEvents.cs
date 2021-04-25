using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for Merchant/Store objects.
  /// </summary>
  public static class StoreEvents
  {
    [GameEvent(EventScriptType.StoreOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwStore"/> being open.
      /// </summary>
      public NwStore Store { get; } = NWScript.OBJECT_SELF.ToNwObject<NwStore>();

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that last opened this store.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastOpenedBy().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Store;
    }

    [GameEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwStore"/> being closed.
      /// </summary>
      public NwStore Store { get; } = NWScript.OBJECT_SELF.ToNwObject<NwStore>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that last closed this <see cref="NwStore"/>.<br/>
      /// If this is a disconnecting player, this value will be a <see cref="NwCreature"/>. See <see cref="IsDisconnectingPlayer"/> to determine this state.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets a value indicating whether the <see cref="Creature"/> is a player leaving the server.
      /// </summary>
      public bool IsDisconnectingPlayer
      {
        get
        {
          if (Creature is not NwPlayer)
          {
            string objectId = Creature.ToString();
            return objectId.Length == 8 && objectId.StartsWith("7ff");
          }

          return false;
        }
      }

      NwObject IEvent.Context => Store;
    }
  }
}
