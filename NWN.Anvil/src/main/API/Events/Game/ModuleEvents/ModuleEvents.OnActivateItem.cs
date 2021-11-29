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
    /// Triggered when an item that has the item property spell "Unique Power" (targeted) or "Unique Power - Self Only" (self) casts its spell.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnActivateItem)]
    public sealed class OnActivateItem : IEvent
    {
      public NwItem ActivatedItem { get; } = NWScript.GetItemActivated().ToNwObject<NwItem>();

      public NwCreature ItemActivator { get; } = NWScript.GetItemActivator().ToNwObject<NwCreature>();

      public NwGameObject TargetObject { get; } = NWScript.GetItemActivatedTarget().ToNwObject<NwGameObject>();

      public Location TargetLocation { get; } = NWScript.GetItemActivatedTargetLocation();

      NwObject IEvent.Context
      {
        get => ItemActivator;
      }

      public static void Signal(NwItem item, Location targetLocation, NwGameObject targetObject = null)
      {
        Event nwEvent = NWScript.EventActivateItem(item, targetLocation, targetObject);
        NWScript.SignalEvent(NwModule.Instance, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnActivateItem"/>
    public event Action<ModuleEvents.OnActivateItem> OnActivateItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnActivateItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnActivateItem, GameEventFactory>(value);
    }
  }

  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="ModuleEvents.OnActivateItem"/>
    public event Action<ModuleEvents.OnActivateItem> OnActivateItem
    {
      add => EventService.Subscribe<ModuleEvents.OnActivateItem, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnActivateItem, GameEventFactory>(this, value);
    }
  }
}
