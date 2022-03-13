namespace Anvil.API
{
  public sealed class ProgrammedEffectTableEntry : ITwoDimArrayEntry
  {
    private const int ParamCount = 8;

    private const string ParamPrefix = "Param";
    private readonly float?[] floatParamValues = new float?[ParamCount];
    private readonly int?[] intParamValues = new int?[ParamCount];

    private readonly string[] stringParamValues = new string[ParamCount];

    public string Label { get; private set; }
    public int RowIndex { get; init; }

    public ProgFxType? Type { get; private set; }

    public float? GetParamFloat(int param)
    {
      return floatParamValues[param - 1];
    }

    public int? GetParamInt(int param)
    {
      return intParamValues[param - 1];
    }

    public string GetParamString(int param)
    {
      return stringParamValues[param - 1];
    }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("Label");
      Type = entry.GetEnum<ProgFxType>("Type");

      for (int i = 0; i < ParamCount; i++)
      {
        string columnName = ParamPrefix + (i + 1);
        stringParamValues[i] = entry.GetString(columnName);
        floatParamValues[i] = entry.GetFloat(columnName);
        intParamValues[i] = entry.GetInt(columnName);
      }
    }
  }
}
