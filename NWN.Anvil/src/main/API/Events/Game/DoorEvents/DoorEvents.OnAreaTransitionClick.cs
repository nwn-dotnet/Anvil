using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific door.
  /// </summary>
  public static partial class DoorEvents
  {
    [GameEvent(EventScriptType.DoorOnClicked)]
    public sealed class OnAreaTransitionClick : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that clicked the transition.
      /// </summary>
      public NwPlayer ClickedBy { get; } = NWScript.GetClickingObject().ToNwPlayer()!;

      /// <summary>
      /// Gets the <see cref="NwDoor"/> that has the transition.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the transition target for this <see cref="NwDoor"/>.
      /// </summary>
      public NwStationary TransitionTarget { get; } = NWScript.GetTransitionTarget(NWScript.OBJECT_SELF).ToNwObject<NwStationary>()!;

      NwObject IEvent.Context => Door;

      /// <summary>
      /// Sets the graphic shown when a PC moves between two different areas in a module.
      /// </summary>
      /// <param name="transition">The transition to use.</param>
      public void SetAreaTransitionBMP(AreaTransition transition)
      {
        if (transition == AreaTransition.UserDefined)
        {
          throw new ArgumentOutOfRangeException(nameof(transition), "Use the string overload instead if wanting to use a user defined transition.");
        }

        NWScript.SetAreaTransitionBMP((int)transition);
      }

      /// <summary>
      /// Sets the graphic shown when a PC moves between two different areas in a module.
      /// </summary>
      /// <param name="transition">The file name (.bmp) to use for the area transition bitmap.</param>
      public void SetAreaTransitionBMP(string transition)
      {
        NWScript.SetAreaTransitionBMP((int)AreaTransition.UserDefined, transition);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnAreaTransitionClick"/>
    public event Action<DoorEvents.OnAreaTransitionClick> OnAreaTransitionClick
    {
      add => EventService.Subscribe<DoorEvents.OnAreaTransitionClick, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnAreaTransitionClick, GameEventFactory>(this, value);
    }
  }
}
