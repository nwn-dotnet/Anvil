namespace NWN.API
{
  public interface ILocalVariableConverter {}

  //! ## Examples
  //! @include DateTimeLocalVariableConverter.cs

  /// <summary>
  /// The interface used in conjunction with the <see cref="LocalVariableConverterAttribute"/> to implement a variable converter for the specified type.
  /// </summary>
  /// <typeparam name="T">The type of variable handled by this converter.</typeparam>
  public interface ILocalVariableConverter<T> : ILocalVariableConverter
  {
    T GetLocal(NwObject nwObject, string name);

    void SetLocal(NwObject nwObject, string name, T value);

    void ClearLocal(NwObject nwObject, string name);
  }
}
