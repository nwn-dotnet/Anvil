using System;
using NWM.API;
using NWN;

namespace NWM.Core
{
  public partial class EventManager
  {
    public event ClientEventEvent OnClientEnter;
    public delegate void ClientEventEvent(NwPlayer entered);

    private void NWNAcquireItem()
    {
      //x2_mod_def_aqu
    }

    private void NWNActivateItem()
    {
      //x2_mod_def_act
    }

    private void NWNClientEnter()
    {
      //x3_mod_def_enter
      NwPlayer player = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();
      OnClientEnter?.Invoke(player);
    }

    private void NWNModuleLoad()
    {
      //x2_mod_def_load
    }

    private void NWNPlayerDeath()
    {
      //nw_o0_death
    }

    private void NWNPlayerDying()
    {
      //nw_o0_dying
    }

    private void NWNPlayerEquipItem()
    {
      //x2_mod_def_equ
    }

    private void NWNPlayerRespawn()
    {
      //nw_o0_respawn
    }

    private void NWNPlayerRest()
    {
      //x2_mod_def_rest
    }

    private void NWNPlayerUnEquipItem()
    {
      //x2_mod_def_unequ
    }

    private void NWNUnAcquireItem()
    {
      //x2_mod_def_unequ
    }

    [ScriptHandler("client_enter")]
    private void ClientEntered()
    {

    }
  }
}