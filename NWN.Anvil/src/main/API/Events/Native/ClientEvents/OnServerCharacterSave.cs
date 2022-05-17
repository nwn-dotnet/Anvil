using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
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
    public NwPlayer Player { get; private init; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the character should be prevented from being saved.
    /// </summary>
    public bool PreventSave { get; set; }

    NwObject? IEvent.Context => Player.ControlledCreature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<SaveServerCharacterHook> Hook { get; set; } = null!;

      private delegate int SaveServerCharacterHook(void* pPlayer, int bBackupPlayer);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, int> pHook = &OnSaveServerCharacter;
        Hook = HookService.RequestHook<SaveServerCharacterHook>(pHook, FunctionsLinux._ZN10CNWSPlayer19SaveServerCharacterEi, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnSaveServerCharacter(void* pPlayer, int bBackupPlayer)
      {
        OnServerCharacterSave eventData = ProcessEvent(new OnServerCharacterSave
        {
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
        });

        return !eventData.PreventSave ? Hook.CallOriginal(pPlayer, bBackupPlayer) : 0;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnServerCharacterSave"/>
    public event Action<OnServerCharacterSave> OnServerCharacterSave
    {
      add => EventService.Subscribe<OnServerCharacterSave, OnServerCharacterSave.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnServerCharacterSave, OnServerCharacterSave.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnServerCharacterSave"/>
    public event Action<OnServerCharacterSave> OnServerCharacterSave
    {
      add => EventService.SubscribeAll<OnServerCharacterSave, OnServerCharacterSave.Factory>(value);
      remove => EventService.UnsubscribeAll<OnServerCharacterSave, OnServerCharacterSave.Factory>(value);
    }
  }
}
