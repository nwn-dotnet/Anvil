using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// The abstract base for all NUI widgets - the building blocks for creating NUI windows.
  /// </summary>
  [JsonPolymorphic]
  public abstract class NuiWidget : NuiElement;
}
