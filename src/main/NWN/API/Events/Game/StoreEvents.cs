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
      public NwPlayer Player { get; } = NWScript.GetLastOpenedBy().ToNwPlayer();

      NwObject IEvent.Context
      {
        get => Store;
      }
    }

    [GameEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwStore"/> being closed.
      /// </summary>
      public NwStore Store { get; } = NWScript.OBJECT_SELF.ToNwObject<NwStore>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that last closed this <see cref="NwStore"/>.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context
      {
        get => Store;
      }
    }
  }
}
