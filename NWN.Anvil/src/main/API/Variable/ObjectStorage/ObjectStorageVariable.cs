namespace Anvil.API
{
  public abstract class ObjectStorageVariable<T> : ObjectVariable<T>
  {
    protected string Key => VariableTypePrefix + Name;

    protected abstract string ObjectStoragePrefix { get; }

    protected abstract bool Persist { get; }

    protected abstract string VariableTypePrefix { get; }
  }
}
