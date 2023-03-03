using System;
using System.Runtime.CompilerServices;
using NWN.Core;

namespace Anvil.API
{
  public sealed class CampaignVariableEnum<T> : CampaignVariable<T> where T : struct, Enum
  {
    public CampaignVariableEnum()
    {
      if (Unsafe.SizeOf<T>() != Unsafe.SizeOf<int>())
      {
        throw new ArgumentOutOfRangeException(nameof(T), "Specified enum must be backed by a signed int32 (int)");
      }
    }

    public override T Value
    {
      get
      {
        int value = NWScript.GetCampaignInt(Campaign, Name, Player?.ControlledCreature);
        return Unsafe.As<int, T>(ref value);
      }
      set
      {
        int newValue = Unsafe.As<T, int>(ref value);
        NWScript.SetCampaignInt(Campaign, Name, newValue, Player?.ControlledCreature);
      }
    }
  }
}
