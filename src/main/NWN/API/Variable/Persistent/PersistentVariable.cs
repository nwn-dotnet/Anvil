namespace NWN.API
{
  public abstract class PersistentVariable<T> : ObjectVariable<T>
  {
    protected const string Prefix = "NWNX_Object"; // For NWNX Compatibility
  }
}
