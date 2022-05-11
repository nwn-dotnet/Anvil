using NWN.Core;

namespace Anvil.API
{
  public sealed class LocalVariableString : LocalVariable<string>
  {
    public override string? Value
    {
      get => HasValue ? NWScript.GetLocalString(Object, Name) : null;
      set => NWScript.SetLocalString(Object, Name, value ?? string.Empty);
    }

    public override void Delete()
    {
      NWScript.DeleteLocalString(Object, Name);
    }
  }
}
