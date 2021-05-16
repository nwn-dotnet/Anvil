using System.Runtime.InteropServices;
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

    NwObject IEvent.Context => Player.ControlledCreature;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.SaveServerCharacterHook>
    {
      internal delegate int SaveServerCharacterHook(void* pPlayer, int bBackupPlayer);

      protected override FunctionHook<SaveServerCharacterHook> RequestHook()
      {
        delegate* unmanaged<void*, int, int> pHook = &OnSaveServerCharacter;
        return HookService.RequestHook<SaveServerCharacterHook>(pHook, FunctionsLinux._ZN10CNWSPlayer19SaveServerCharacterEi, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnSaveServerCharacter(void* pPlayer, int bBackupPlayer)
      {
        OnServerCharacterSave eventData = ProcessEvent(new OnServerCharacterSave
        {
          Player = new CNWSPlayer(pPlayer, false).ToNwPlayer()
        });

        return !eventData.PreventSave ? Hook.CallOriginal(pPlayer, bBackupPlayer) : 0;
      }
    }
  }
}
