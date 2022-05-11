using System.Numerics;

namespace Anvil.API
{
  public sealed class PlaceableTableEntry : ITwoDimArrayEntry
  {
    public bool? HasBodyBag { get; private set; }

    public string? Label { get; private set; }

    public LightColorTableEntry? LightColor { get; private set; }

    public Vector3? LightOffset { get; private set; }

    public string? LowGore { get; private set; }

    public string? ModelName { get; private set; }

    public string? Reflection { get; private set; }

    public int RowIndex { get; init; }

    public ShadowSize? ShadowSize { get; private set; }

    public PlaceableSoundTableEntry? SoundType { get; private set; }

    public bool? StaticAllowed { get; private set; }

    public StrRef? StrRef { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("Label");
      StrRef = entry.GetStrRef("StrRef");
      ModelName = entry.GetString("ModelName");
      LightColor = entry.GetTableEntry("LightColor", NwGameTables.LightColorTable);
      LightOffset = entry.GetVector3("LightOffsetX", "LightOffsetY", "LightOffsetZ");
      SoundType = entry.GetTableEntry("SoundAppType", NwGameTables.PlaceableSoundTable);
      ShadowSize = entry.GetEnum<ShadowSize>("ShadowSize");
      HasBodyBag = entry.GetBool("BodyBag");
      LowGore = entry.GetString("LowGore");
      Reflection = entry.GetString("Reflection");
      StaticAllowed = entry.GetBool("Static");
    }
  }
}
