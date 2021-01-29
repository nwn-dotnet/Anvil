using NWN.Native.API;

namespace NWN.API
{
  public unsafe class DebugOptions
  {
    internal DebugOptions() {}

    public bool EnableCombatDebugging
    {
      get => (*NWNXLib.EnableCombatDebugging()).ToBool();
      set => *NWNXLib.EnableCombatDebugging() = value.ToInt();
    }

    public bool EnableSavingThrowDebugging
    {
      get => (*NWNXLib.EnableSavingThrowDebugging()).ToBool();
      set => *NWNXLib.EnableSavingThrowDebugging() = value.ToInt();
    }

    public bool EnableMovementSpeedDebugging
    {
      get => (*NWNXLib.EnableMovementSpeedDebugging()).ToBool();
      set => *NWNXLib.EnableMovementSpeedDebugging() = value.ToInt();
    }

    public bool EnableHitDieDebugging
    {
      get => (*NWNXLib.EnableHitDieDebugging()).ToBool();
      set => *NWNXLib.EnableHitDieDebugging() = value.ToInt();
    }
  }
}
