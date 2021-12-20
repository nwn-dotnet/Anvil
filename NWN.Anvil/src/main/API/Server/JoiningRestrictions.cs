using NWN.Native.API;

namespace Anvil.API
{
  public sealed class JoiningRestrictions
  {
    private readonly CJoiningRestrictions joiningRestrictions;

    internal JoiningRestrictions(CJoiningRestrictions joiningRestrictions)
    {
      this.joiningRestrictions = joiningRestrictions;
    }

    public bool AllowLocalVaultCharacters => joiningRestrictions.bAllowLocalVaultChars.ToBool();

    public int MaxLevel => joiningRestrictions.nMaxLevel;

    public int MinLevel => joiningRestrictions.nMinLevel;
  }
}
