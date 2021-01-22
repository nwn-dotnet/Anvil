using NWN.Native.API;

namespace NWN.API
{
  public class DebugOptions
  {
    internal DebugOptions() {}

    public bool EnableCombatDebugging
    {
      get => NWNXLib.EnableCombatDebugging().Read().ToBool();
      set => NWNXLib.EnableCombatDebugging().Write(value.ToInt());
    }

    public bool EnableSavingThrowDebugging
    {
      get => NWNXLib.EnableSavingThrowDebugging().Read().ToBool();
      set => NWNXLib.EnableSavingThrowDebugging().Write(value.ToInt());
    }

    public bool EnableMovementSpeedDebugging
    {
      get => NWNXLib.EnableMovementSpeedDebugging().Read().ToBool();
      set => NWNXLib.EnableMovementSpeedDebugging().Write(value.ToInt());
    }

    public bool EnableHitDieDebugging
    {
      get => NWNXLib.EnableHitDieDebugging().Read().ToBool();
      set => NWNXLib.EnableHitDieDebugging().Write(value.ToInt());
    }
  }
}
