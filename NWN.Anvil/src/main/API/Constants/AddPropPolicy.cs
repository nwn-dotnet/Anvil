namespace Anvil.API
{
  public enum AddPropPolicy
  {
    /// <summary>
    /// Always add the property, regardless of if a matching one already exists.
    /// </summary>
    IgnoreExisting,

    /// <summary>
    /// Remove and replace matching existing properties with the new property.
    /// </summary>
    ReplaceExisting,

    /// <summary>
    /// Keep existing matching properties, don't add the new property if it already exists.
    /// </summary>
    KeepExisting,
  }
}
