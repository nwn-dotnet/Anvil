namespace NWN.API
{
  public interface ILocalVariableConverter {}

  public interface ILocalVariableConverter<T> : ILocalVariableConverter
  {
    T GetLocal(NwObject nwObject, string name);
    void SetLocal(NwObject nwObject, string name, T value);
    void ClearLocal(NwObject nwObject, string name);
  }
}