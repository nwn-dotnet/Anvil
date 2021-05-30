using NWN.Native.API;

namespace NWN.API
{
  public sealed class JoiningRestrictions
  {
    private readonly CJoiningRestrictions joiningRestrictions;

    internal JoiningRestrictions(CJoiningRestrictions joiningRestrictions)
    {
      this.joiningRestrictions = joiningRestrictions;
    }

    public int MaxLevel
    {
      get => joiningRestrictions.nMaxLevel;
    }
  }
}
