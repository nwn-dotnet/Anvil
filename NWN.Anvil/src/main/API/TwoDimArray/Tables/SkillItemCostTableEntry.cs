namespace Anvil.API
{
  public sealed class SkillItemCostTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    /// <summary>
    /// The maximum item value that can use the skill checks on this row.
    /// </summary>
    public int? DeviceCostMax { get; set; }

    /// <summary>
    /// The skill check DC to use items that do not meet class requirements.
    /// </summary>
    public int? ClassSkillRequirement { get; set; }

    /// <summary>
    /// The skill check DC to use items that do not meet race requirements.
    /// </summary>
    public int? RaceSkillRequirement { get; set; }

    /// <summary>
    /// The skill check DC to use items that do not meet alignment requirements.
    /// </summary>
    public int? AlignmentSkillRequirement { get; set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      DeviceCostMax = entry.GetInt("DeviceCostMax");
      ClassSkillRequirement = entry.GetInt("SkillReq_Class");
      RaceSkillRequirement = entry.GetInt("SkillReq_Race");
      AlignmentSkillRequirement = entry.GetInt("SkillReq_Align");
    }
  }
}
