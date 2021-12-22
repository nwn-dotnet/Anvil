namespace Anvil.API
{
  public abstract class PersistentVariable<T> : ObjectVariable<T>
  {
    protected string Key => KeyPrefix + Name;

    protected abstract string KeyPrefix { get; }

    protected virtual string Prefix => "NWNX_Object"; // For NWNX Compatibility
  }
}
