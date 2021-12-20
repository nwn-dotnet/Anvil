using System.Linq;

namespace Anvil.API
{
  public abstract class LocalVariable<T> : ObjectVariable<T>
  {
    public override bool HasValue => Object.ScriptVarTable.m_vars.Keys.Any(scriptVar => scriptVar.ToString() == Name);
  }
}
