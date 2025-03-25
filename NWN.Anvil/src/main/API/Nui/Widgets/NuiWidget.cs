using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// The abstract base for all NUI widgets - the building blocks for creating NUI windows.
  /// </summary>
  [JsonPolymorphic]
  [JsonDerivedType(typeof(NuiButton))]
  [JsonDerivedType(typeof(NuiButtonImage))]
  [JsonDerivedType(typeof(NuiButtonSelect))]
  [JsonDerivedType(typeof(NuiChart))]
  [JsonDerivedType(typeof(NuiCheck))]
  [JsonDerivedType(typeof(NuiColorPicker))]
  [JsonDerivedType(typeof(NuiCombo))]
  [JsonDerivedType(typeof(NuiImage))]
  [JsonDerivedType(typeof(NuiLabel))]
  [JsonDerivedType(typeof(NuiOptions))]
  [JsonDerivedType(typeof(NuiProgress))]
  [JsonDerivedType(typeof(NuiSlider))]
  [JsonDerivedType(typeof(NuiSliderFloat))]
  [JsonDerivedType(typeof(NuiSpacer))]
  [JsonDerivedType(typeof(NuiText))]
  [JsonDerivedType(typeof(NuiTextEdit))]
  [JsonDerivedType(typeof(NuiToggles))]
  public abstract class NuiWidget : NuiElement;
}
