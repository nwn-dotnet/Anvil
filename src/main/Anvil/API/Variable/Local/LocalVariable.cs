using System.Linq;

namespace NWN.API
{
  public abstract class LocalVariable<T> : ObjectVariable<T>
  {
    public override bool HasValue
    {
      get => Object.ScriptVarTable.m_vars.Keys.Any(scriptVar => scriptVar.ToString() == Name);
    }
  }
}
