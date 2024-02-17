using System;
using System.Linq;
using System.Numerics;
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
    /// Triggered when a player clicks on a particular GUI interface.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerGuiEvent)]
    public sealed class OnPlayerGuiEvent : IEvent
    {
      private readonly int integerEventData = NWScript.GetLastGuiEventInteger();

      /// <summary>
      /// Gets the chat bar channel that is selected. Only valid in <see cref="GuiEventType.ChatBarFocus"/> and <see cref="GuiEventType.ChatBarUnFocus"/> type events.
      /// </summary>
      public ChatBarChannel ChatBarChannel => (ChatBarChannel)integerEventData;

      /// <summary>
      /// Gets the effect icon that was selected. Only valid in <see cref="GuiEventType.EffectIconClick"/> events.
      /// </summary>
      public EffectIconTableEntry? EffectIcon => NwGameTables.EffectIconTable.ElementAtOrDefault(integerEventData);

      /// <summary>
      /// Gets the object data associated with this GUI event.
      /// </summary>
      /// <remarks>
      /// <see cref="GuiEventType.MinimapMapPinClick"/>: The waypoint the map note is attached to.
      /// <see cref="GuiEventType.CharacterSheetSkillClick"/>: The owner of the character sheet.<br/>
      /// <see cref="GuiEventType.CharacterSheetFeatClick"/>: The owner of the character sheet.<br/>
      /// <see cref="GuiEventType.PlayerListPlayerClick"/>: The player that was clicked.<br/>
      /// <see cref="GuiEventType.PartyBarPortraitClick"/>: The creature that was clicked.<br/>
      /// <see cref="GuiEventType.DisabledPanelAttemptOpen"/>: For <see cref="GUIPanel.CharacterSheet"/>, the owner of the character sheet. For GUIPanel.Examine*, the object being examined.<br/>
      /// <see cref="GuiEventType.ExamineObject"/>: The object being examined.<br/>
      /// <see cref="GuiEventType.ChatlogPortraitClick"/>: The owner of the portrait that was clicked.<br/>
      /// <see cref="GuiEventType.PlayerlistPlayerTell"/>: The selected player.<br/>
      /// </remarks>
      public NwObject EventObject { get; } = NWScript.GetLastGuiEventObject().ToNwObject()!;

      /// <summary>
      /// Gets the <see cref="GuiEventType"/> that was triggered.
      /// </summary>
      public GuiEventType EventType { get; } = (GuiEventType)NWScript.GetLastGuiEventType();

      /// <summary>
      /// Gets the feat that was selected. Only valid in <see cref="GuiEventType.CharacterSheetFeatClick"/> events.
      /// </summary>
      public NwFeat FeatSelection => NwFeat.FromFeatId(integerEventData)!;

      /// <summary>
      /// Gets the GUI panel that attempted to be opened. Only valid in <see cref="GuiEventType.DisabledPanelAttemptOpen"/> events.
      /// </summary>
      public GUIPanel OpenedPanel => (GUIPanel)integerEventData;

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that triggered this event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastGuiEventPlayer().ToNwPlayer()!;

      /// <summary>
      /// Gets the skill that was selected. Only valid in <see cref="GuiEventType.CharacterSheetSkillClick"/> events.
      /// </summary>
      public NwSkill SkillSelection => NwSkill.FromSkillId(integerEventData)!;

      /// <summary>
      /// Gets the vector payload included in this gui event. Only valid in <see cref="GuiEventType.RadialOpen"/> events.
      /// </summary>
      public Vector3 Vector => NWScript.GetLastGuiEventVector();

      NwObject? IEvent.Context => Player.ControlledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerGuiEvent"/>
    public event Action<ModuleEvents.OnPlayerGuiEvent> OnPlayerGuiEvent
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerGuiEvent, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerGuiEvent, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerGuiEvent"/>
    public event Action<ModuleEvents.OnPlayerGuiEvent> OnPlayerGuiEvent
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerGuiEvent, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerGuiEvent, GameEventFactory>(ControlledCreature, value);
    }
  }
}
