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
    /// Triggered when a <see cref="NwItem"/> is removed from a <see cref="NwCreature"/>'s inventory.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnLoseItem)]
    public sealed class OnUnacquireItem : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwItem"/> that was lost by <see cref="NwCreature"/>.
      /// </summary>
      public NwItem Item { get; } = NWScript.GetModuleItemLost().ToNwObject<NwItem>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that lost the <see cref="NwItem"/>.
      /// </summary>
      public NwCreature LostBy { get; } = NWScript.GetModuleItemLostBy().ToNwObject<NwCreature>();

      NwObject IEvent.Context => LostBy;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnUnacquireItem"/>
    public event Action<ModuleEvents.OnUnacquireItem> OnUnacquireItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnUnacquireItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnUnacquireItem, GameEventFactory>(value);
    }
  }

  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="ModuleEvents.OnUnacquireItem"/>
    public event Action<ModuleEvents.OnUnacquireItem> OnUnacquireItem
    {
      add => EventService.Subscribe<ModuleEvents.OnUnacquireItem, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnUnacquireItem, GameEventFactory>(this, value);
    }
  }
}
