namespace Anvil.API
{
  public abstract class PersistentVariable<T> : ObjectVariable<T>
  {
    protected string Key
    {
      get => KeyPrefix + Name;
    }

    protected abstract string KeyPrefix { get; }
    protected virtual string Prefix
    {
      get => "NWNX_Object"; // For NWNX Compatibility
    }
  }
}
