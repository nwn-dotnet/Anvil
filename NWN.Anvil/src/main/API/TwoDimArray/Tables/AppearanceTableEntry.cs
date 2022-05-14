namespace Anvil.API
{
  /// <summary>
  /// A creature appearance table entry (appearance.2da)
  /// </summary>
  public sealed class AppearanceTableEntry : ITwoDimArrayEntry
  {
    public bool? AbortOnParry { get; private set; }

    public int? AppearanceSoundSet { get; private set; }

    public string? BloodColor { get; private set; }

    public BodyBagTableEntry? BodyBag { get; private set; }

    public float? CreaturePersonalSpace { get; private set; }

    public string? EnvironmentMap { get; private set; }

    public int? FootstepType { get; private set; }

    public bool? HasArms { get; private set; }

    public bool? HasLegs { get; private set; }

    public int? HeadArcHorizontal { get; private set; }

    public int? HeadArcVertical { get; private set; }

    public string? HeadName { get; private set; }

    public bool? HeadTrack { get; private set; }

    public float? Height { get; private set; }

    public float? HelmetScaleF { get; private set; }

    public float? HelmetScaleM { get; private set; }

    public float? HitDistance { get; private set; }

    public string? Label { get; private set; }

    public string? ModelType { get; private set; }

    public string? MovementRate { get; private set; }

    public string? Name { get; private set; }

    public int? PerceptionDistance { get; private set; }

    public float? PersonalSpace { get; private set; }

    public string? Portrait { get; private set; }

    public float? PreferredAttackDistance { get; private set; }

    public string? Race { get; private set; }

    public int? RacialType { get; private set; }

    public int RowIndex { get; init; }

    public float? RunDistance { get; private set; }

    public int? SizeCategory { get; private set; }

    public StrRef? StrRef { get; private set; }

    public bool? Targetable { get; private set; }

    public string? TargetHeight { get; private set; }

    public float? WalkDistance { get; private set; }

    public float? WeaponScale { get; private set; }

    public float? WingTailScale { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("LABEL");
      StrRef = entry.GetStrRef("STRING_REF");
      Name = entry.GetString("NAME");
      Race = entry.GetString("RACE");
      EnvironmentMap = entry.GetString("ENVMAP");
      BloodColor = entry.GetString("BLOODCOLR");
      ModelType = entry.GetString("MODELTYPE");
      WeaponScale = entry.GetFloat("WEAPONSCALE");
      WingTailScale = entry.GetFloat("WING_TAIL_SCALE");
      HelmetScaleM = entry.GetFloat("HELMET_SCALE_M");
      HelmetScaleF = entry.GetFloat("HELMET_SCALE_F");
      MovementRate = entry.GetString("MOVERATE");
      WalkDistance = entry.GetFloat("WALKDIST");
      RunDistance = entry.GetFloat("RUNDIST");
      PersonalSpace = entry.GetFloat("PERSPACE");
      CreaturePersonalSpace = entry.GetFloat("CREPERSPACE");
      Height = entry.GetFloat("HEIGHT");
      HitDistance = entry.GetFloat("HITDIST");
      PreferredAttackDistance = entry.GetFloat("PREFATCKDIST");
      TargetHeight = entry.GetString("TARGETHEIGHT");
      AbortOnParry = entry.GetBool("ABORTONPARRY");
      RacialType = entry.GetInt("RACIALTYPE");
      HasLegs = entry.GetBool("HASLEGS");
      HasArms = entry.GetBool("HASARMS");
      Portrait = entry.GetString("PORTRAIT");
      SizeCategory = entry.GetInt("SIZECATEGORY");
      PerceptionDistance = entry.GetInt("PERCEPTIONDIST");
      FootstepType = entry.GetInt("FOOTSTEPTYPE");
      AppearanceSoundSet = entry.GetInt("SOUNDAPPTYPE");
      HeadTrack = entry.GetBool("HEADTRACK");
      HeadArcHorizontal = entry.GetInt("HEAD_ARC_H");
      HeadArcVertical = entry.GetInt("HEAD_ARC_V");
      HeadName = entry.GetString("HEAD_NAME");
      BodyBag = entry.GetTableEntry("BODY_BAG", NwGameTables.BodyBagTable);
      Targetable = entry.GetBool("TARGETABLE");
    }
  }
}
