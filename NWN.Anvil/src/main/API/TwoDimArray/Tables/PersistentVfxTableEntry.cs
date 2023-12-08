using System;
using System.Linq;

namespace Anvil.API
{
  public sealed class PersistentVfxTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public string? Label { get; private set; }

    public string? Shape { get; private set; }

    public float? Radius { get; private set; }

    public float? Width { get; private set; }

    public float? Length { get; private set; }

    public string? OnEnter { get; private set; }

    public string? OnExit { get; private set; }

    public string? OnHeartbeat { get; private set; }

    public bool? OrientWithGround { get; private set; }

    public VisualEffectTableEntry? DurationVfx { get; private set; }

    public string? Model01 { get; private set; }

    public string? Model02 { get; private set; }

    public string? Model03 { get; private set; }

    public int? NumAct01 { get; private set; }

    public int? NumAct02 { get; private set; }

    public int? NumAct03 { get; private set; }

    public TimeSpan? Duration01 { get; private set; }

    public TimeSpan? Duration02 { get; private set; }

    public TimeSpan? Duration03 { get; private set; }

    public float? EdgeWeight01 { get; private set; }

    public float? EdgeWeight02 { get; private set; }

    public float? EdgeWeight03 { get; private set; }

    public string? SoundImpact { get; private set; }

    public string? SoundDuration { get; private set; }

    public string? SoundCessation { get; private set; }

    public string? SoundOneShot { get; private set; }

    public float? SoundOneShotPercentage { get; private set; }

    public string? ModelMin01 { get; private set; }

    public string? ModelMin02 { get; private set; }

    public string? ModelMin03 { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("LABEL");
      Shape = entry.GetString("SHAPE");
      Radius = entry.GetFloat("RADIUS");
      Width = entry.GetFloat("WIDTH");
      Length = entry.GetFloat("LENGTH");
      OnEnter = entry.GetString("ONENTER");
      OnExit = entry.GetString("ONEXIT");
      OnHeartbeat = entry.GetString("HEARTBEAT");
      OrientWithGround = entry.GetBool("OrientWithGround");
      DurationVfx = entry.GetTableEntry("DurationVFX", NwGameTables.VisualEffectTable);
      Model01 = entry.GetString("MODEL01");
      Model02 = entry.GetString("MODEL02");
      Model03 = entry.GetString("MODEL03");
      NumAct01 = entry.GetInt("NUMACT01");
      NumAct02 = entry.GetInt("NUMACT02");
      NumAct03 = entry.GetInt("NUMACT03");
      Duration01 = GetDuration(entry, "DURATION01");
      Duration02 = GetDuration(entry, "DURATION02");
      Duration03 = GetDuration(entry, "DURATION03");
      EdgeWeight01 = entry.GetFloat("EDGEWGHT01");
      EdgeWeight02 = entry.GetFloat("EDGEWGHT02");
      EdgeWeight03 = entry.GetFloat("EDGEWGHT03");
      SoundImpact = entry.GetString("SoundImpact");
      SoundDuration = entry.GetString("SoundDuration");
      SoundCessation = entry.GetString("SoundCessation");
      SoundOneShot = entry.GetString("SoundOneShot");
      SoundOneShotPercentage = entry.GetFloat("SoundOneShotPercentage");
      ModelMin01 = entry.GetString("MODELMIN01");
      ModelMin02 = entry.GetString("MODELMIN02");
      ModelMin03 = entry.GetString("MODELMIN03");
    }

    private static TimeSpan? GetDuration(TwoDimArrayEntry entry, string columnName)
    {
      int? timeMs = entry.GetInt(columnName);
      return timeMs == null ? null : TimeSpan.FromMilliseconds(timeMs.Value);
    }

    public static implicit operator PersistentVfxTableEntry?(PersistentVfxType vfxType)
    {
      return NwGameTables.PersistentEffectTable.ElementAtOrDefault((int)vfxType);
    }
  }
}
