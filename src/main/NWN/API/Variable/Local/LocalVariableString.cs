using NWN.Core;

namespace NWN.API
{
  public sealed class LocalVariableString : LocalVariable<string>
  {
    public override string Value
    {
      get => NWScript.GetLocalString(Object, Name);
      set => NWScript.SetLocalString(Object, Name, value);
    }

    public override void Delete()
    {
      NWScript.DeleteLocalString(Object, Name);
    }
  }
}
