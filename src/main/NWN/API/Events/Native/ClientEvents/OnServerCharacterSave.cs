using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  /// <summary>
  /// Called when the server is about to save a character to the server vault.<br/>
  /// This is called once for every character when the server is exiting, when a player leaves the server, or when ExportSingleCharacter() &amp; ExportAllCharacters() is called.<br/>
  /// This event can be skipped to prevent the character from being saved.
  /// </summary>
  public sealed class OnServerCharacterSave : IEvent
  {
    /// <summary>
    /// Gets the player that is being saved.
    /// </summary>
    public NwPlayer Player { get; private init; }

    /// <summary>
    /// Gets or sets a value indicating whether the character should be prevented from being saved.
    /// </summary>
    public bool PreventSave { get; set; }

    NwObject IEvent.Context => Player;

    [NativeFunction(NWNXLib.Functions._ZN10CNWSPlayer19SaveServerCharacterEi)]
    internal delegate int SaveServerCharacterHook(IntPtr pPlayer, int bBackupPlayer);

    internal class Factory : NativeEventFactory<SaveServerCharacterHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SaveServerCharacterHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SaveServerCharacterHook>(OnSaveServerCharacter, HookOrder.Early);

      private int OnSaveServerCharacter(IntPtr pPlayer, int bBackupPlayer)
      {
        OnServerCharacterSave eventData = ProcessEvent(new OnServerCharacterSave
        {
          Player = new NwPlayer(new CNWSPlayer(pPlayer, false))
        });

        return !eventData.PreventSave ? Hook.Original.Invoke(pPlayer, bBackupPlayer) : 0;
      }
    }
  }
}
