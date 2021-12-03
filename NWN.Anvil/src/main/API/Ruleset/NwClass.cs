using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A creature/character class.
  /// </summary>
  public sealed class NwClass
  {
    [Inject]
    private static RulesetService RulesetService { get; set; }

    [Inject]
    private static TlkTable TlkTable { get; set; }

    private readonly CNWClass classInfo;

    public NwClass(ClassType classType, CNWClass classInfo)
    {
      this.classInfo = classInfo;
      ClassType = classType;
    }

    public ClassType ClassType { get; }

    /// <summary>
    /// Gets the name of this class as shown on the character sheet.
    /// </summary>
    public string Name
    {
      get => TlkTable.GetSimpleString(classInfo.m_nName);
    }

    /// <summary>
    /// Gets the name of this class, in lowercase.
    /// </summary>
    public string NameLower
    {
      get => TlkTable.GetSimpleString(classInfo.m_nNameLower);
    }

    /// <summary>
    /// Gets the name of this class, in plural form.
    /// </summary>
    public string NamePlural
    {
      get => TlkTable.GetSimpleString(classInfo.m_nNamePlural);
    }

    /// <summary>
    /// Gets the description name of this class.
    /// </summary>
    public string Description
    {
      get => TlkTable.GetSimpleString(classInfo.m_nDescription);
    }

    public static NwClass FromClassId(byte classId)
    {
      return classId != IntegerExtensions.AsByte(-1) ? FromClassId((int)classId) : null;
    }

    public static NwClass FromClassId(ushort classId)
    {
      return classId != IntegerExtensions.AsUShort(-1) ? FromClassId((int)classId) : null;
    }

    public static NwClass FromClassId(int classId)
    {
      return classId >= 0 && classId < RulesetService.Classes.Count ? RulesetService.Classes[classId] : null;
    }

    public static NwClass FromClassType(ClassType classType)
    {
      return RulesetService.Classes[(int)classType];
    }
  }
}
