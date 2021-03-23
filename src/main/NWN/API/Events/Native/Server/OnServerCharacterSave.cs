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
    public NwPlayer Player { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the character should be prevented from being saved.
    /// </summary>
    public bool PreventSave { get; set; }

    NwObject IEvent.Context => Player;

    private OnServerCharacterSave(CNWSPlayer player)
    {
      Player = new NwPlayer(player);
    }

    [NativeFunction(NWNXLib.Functions._ZN10CNWSPlayer19SaveServerCharacterEi)]
    internal delegate int SaveServerCharacterHook(IntPtr pPlayer, int bBackupPlayer);

    internal class Factory : NativeEventFactory<SaveServerCharacterHook>
    {
      public Factory(HookService hookService) : base(hookService) {}

      protected override int FunctionHookOrder { get; } = HookOrder.Early;

      protected override SaveServerCharacterHook Handler => OnSaveServerCharacter;

      private int OnSaveServerCharacter(IntPtr pPlayer, int bBackupPlayer)
      {
        OnServerCharacterSave eventData = ProcessEvent(new OnServerCharacterSave(new CNWSPlayer(pPlayer, false)));
        return !eventData.PreventSave ? FunctionHook.Original.Invoke(pPlayer, bBackupPlayer) : 0;
      }
    }
  }
}
