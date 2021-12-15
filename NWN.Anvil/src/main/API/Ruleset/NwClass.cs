using System.Linq;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A creature/character class definition.
  /// </summary>
  public sealed class NwClass
  {
    [Inject]
    private static TlkTable TlkTable { get; set; }

    private readonly CNWClass classInfo;

    internal NwClass(byte classId, CNWClass classInfo)
    {
      Id = classId;
      this.classInfo = classInfo;
    }

    /// <summary>
    /// Gets the associated <see cref="Id"/> for this class.
    /// </summary>
    public ClassType ClassType
    {
      get => (ClassType)Id;
    }

    /// <summary>
    /// Gets the description name of this class.
    /// </summary>
    public string Description
    {
      get => TlkTable.GetSimpleString(classInfo.m_nDescription);
    }

    /// <summary>
    /// Gets the id of this class.
    /// </summary>
    public byte Id { get; }

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
    /// Resolves a <see cref="NwClass"/> from a class id.
    /// </summary>
    /// <param name="classId">The id of the class to resolve.</param>
    /// <returns>The associated <see cref="NwClass"/> instance. Null if the class id is invalid.</returns>
    public static NwClass FromClassId(int classId)
    {
      return NwRuleset.Classes.ElementAtOrDefault(classId);
    }

    /// <summary>
    /// Resolves a <see cref="NwClass"/> from a <see cref="Anvil.API.ClassType"/>.
    /// </summary>
    /// <param name="classType">The class type to resolve.</param>
    /// <returns>The associated <see cref="NwClass"/> instance. Null if the class type is invalid.</returns>
    public static NwClass FromClassType(ClassType classType)
    {
      return NwRuleset.Classes.ElementAtOrDefault((int)classType);
    }

    public static implicit operator NwClass(ClassType classType)
    {
      return NwRuleset.Classes.ElementAtOrDefault((int)classType);
    }
  }
}
