using System.Linq;

namespace Anvil.API
{
  public sealed class VisualEffectTableEntry : ITwoDimArrayEntry
  {
    public string? CesHeadConNode { get; private set; }

    public string? CesImpactNode { get; private set; }

    public string? CesRootHugeNode { get; private set; }

    public string? CesRootLargeNode { get; private set; }

    public string? CesRootMediumNode { get; private set; }

    public string? CesRootSmallNode { get; private set; }

    public string? ImpHeadConNode { get; private set; }

    public string? ImpImpactNode { get; private set; }

    public string? ImpRootHugeNode { get; private set; }

    public string? ImpRootLargeNode { get; private set; }

    public string? ImpRootMediumNode { get; private set; }

    public string? ImpRootSmallNode { get; private set; }

    public string? Label { get; private set; }

    public string? LowQualityVariant { get; private set; }

    public string? LowViolenceVariant { get; private set; }

    public bool? OrientWithGround { get; private set; }

    public bool? OrientWithObject { get; private set; }

    public ProgrammedEffectTableEntry? ProgFxCessastion { get; private set; }

    public ProgrammedEffectTableEntry? ProgFxDuration { get; private set; }

    public ProgrammedEffectTableEntry? ProgFxImpact { get; private set; }

    public int RowIndex { get; init; }

    public float? ShakeDelay { get; private set; }

    public float? ShakeDuration { get; private set; }

    public ShakeType? ShakeType { get; private set; }

    public string? SoundCessastion { get; private set; }

    public string? SoundDuration { get; private set; }

    public string? SoundImpact { get; private set; }

    public string? TypeFd { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("Label");
      TypeFd = entry.GetString("Type_FD");
      OrientWithGround = entry.GetBool("OrientWithGround");
      ImpHeadConNode = entry.GetString("Imp_HeadCon_Node");
      ImpImpactNode = entry.GetString("Imp_Impact_Node");
      ImpRootSmallNode = entry.GetString("Imp_Root_S_Node");
      ImpRootMediumNode = entry.GetString("Imp_Root_M_Node");
      ImpRootLargeNode = entry.GetString("Imp_Root_L_Node");
      ImpRootHugeNode = entry.GetString("Imp_Root_H_Node");
      ProgFxImpact = entry.GetTableEntry("ProgFX_Impact", NwGameTables.ProgrammedEffectTable);
      SoundImpact = entry.GetString("SoundImpact");
      ProgFxDuration = entry.GetTableEntry("ProgFX_Duration", NwGameTables.ProgrammedEffectTable);
      SoundDuration = entry.GetString("SoundDuration");
      ProgFxCessastion = entry.GetTableEntry("ProgFX_Cessation", NwGameTables.ProgrammedEffectTable);
      SoundCessastion = entry.GetString("SoundCessastion");
      CesHeadConNode = entry.GetString("Ces_HeadCon_Node");
      CesImpactNode = entry.GetString("Ces_Impact_Node");
      CesRootSmallNode = entry.GetString("Ces_Root_S_Node");
      CesRootMediumNode = entry.GetString("Ces_Root_M_Node");
      CesRootLargeNode = entry.GetString("Ces_Root_L_Node");
      CesRootHugeNode = entry.GetString("Ces_Root_H_Node");
      ShakeType = entry.GetEnum<ShakeType>("ShakeType");
      ShakeDelay = entry.GetFloat("ShakeDelay");
      ShakeDuration = entry.GetFloat("ShakeDuration");
      LowViolenceVariant = entry.GetString("LowViolence");
      LowQualityVariant = entry.GetString("LowQuality");
      OrientWithObject = entry.GetBool("OrientWithObject");
    }

    public static implicit operator VisualEffectTableEntry?(VfxType vfxType)
    {
      return NwGameTables.VisualEffectTable.ElementAtOrDefault((int)vfxType);
    }
  }
}
