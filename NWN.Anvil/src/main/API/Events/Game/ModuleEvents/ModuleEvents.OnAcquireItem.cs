using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static partial class ModuleEvents
  {
    /// <summary>
    /// Triggered whenever an <see cref="NwItem"/> is added to <see cref="NwGameObject"/> inventory.
    /// </summary>
    /// <remarks>
    /// This event fires for all items when a player connects to the server, in addition to item/inventory interactions while playing.<br/>
    /// It will also fire for characters failing ELC. In this case, it is recommended to do an early return in your event handler by checking if <see cref="Item"/> is null.
    /// </remarks>
    [GameEvent(EventScriptType.ModuleOnAcquireItem)]
    public sealed class OnAcquireItem : IEvent
    {
      public OnAcquireItem()
      {
        // Patch player reference due to a reference bug during client enter context
        // See https://github.com/Beamdog/nwn-issues/issues/367
        if (AcquiredBy is null && Item?.Possessor is NwCreature creature)
        {
          AcquiredBy = creature;
        }
      }

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that acquired the <see cref="NwItem"/>.
      /// </summary>
      public NwGameObject? AcquiredBy { get; } = NWScript.GetModuleItemAcquiredBy().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that the <see cref="NwItem"/> was taken from.
      /// </summary>
      public NwGameObject? AcquiredFrom { get; } = NWScript.GetModuleItemAcquiredFrom().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the number of items in the item stack that were just acquired.
      /// </summary>
      public int AmountAcquired { get; } = NWScript.GetModuleItemAcquiredStackSize();

      /// <summary>
      /// Gets the <see cref="NwItem"/> that triggered the event.
      /// </summary>
      /// <remarks>
      /// This property will return null when a character fails ELC. It is recommended to do an early exit if this is null.
      /// </remarks>
      public NwItem? Item { get; } = NWScript.GetModuleItemAcquired().ToNwObject<NwItem>();

      NwObject? IEvent.Context => AcquiredBy;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnAcquireItem"/>
    public event Action<ModuleEvents.OnAcquireItem> OnAcquireItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnAcquireItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnAcquireItem, GameEventFactory>(value);
    }
  }

  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="ModuleEvents.OnAcquireItem"/>
    public event Action<ModuleEvents.OnAcquireItem> OnAcquireItem
    {
      add => EventService.Subscribe<ModuleEvents.OnAcquireItem, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnAcquireItem, GameEventFactory>(this, value);
    }
  }
}
