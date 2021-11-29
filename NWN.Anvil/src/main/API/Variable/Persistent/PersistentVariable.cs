namespace Anvil.API
{
  public abstract class PersistentVariable<T> : ObjectVariable<T>
  {
    protected const string Prefix = "NWNX_Object"; // For NWNX Compatibility

    protected string Key
    {
      get => KeyPrefix + Name;
    }

    protected abstract string KeyPrefix { get; }
  }
}
