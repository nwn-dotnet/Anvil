using NWN.API;
using NWN.API.Constants;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Area
  {
    static Area()
    {
      PluginUtils.AssertPluginExists<AreaPlugin>();
    }

    /// <summary>
    /// Add NwObject to the ExportGIT exclusion list, objects on this list won't be exported when ExportGIT() is called.
    /// </summary>
    public static void AddObjectToExclusionList(NwObject obj) => AreaPlugin.AddObjectToExclusionList(obj);

    /// <summary>
    /// Remove NwObject from the ExportGIT exclusion list.
    /// </summary>
    public static void RemoveObjectFromExclusionList(NwObject obj) => AreaPlugin.RemoveObjectFromExclusionList(obj);

    /// <summary>
    /// Export the .git file of this area to the UserDirectory/nwnx folder, or to the location of alias.
    /// </summary>
    /// <param name="area">The NwArea to check.</param>
    /// <param name="fileName">The filename, 16 characters or less and should be lowercase. If left blank the resref of oArea will be used.</param>
    /// <param name="exportVariables">If true, local variables set on this area will be exported.</param>
    /// <param name="exportUUID">If true, the UUID of this area will be exported.</param>
    /// <param name="objectTypeFilter">Filter object types. These objects will not be exported.</param>
    /// <param name="alias">The alias of the resource directory to add the .git file to. Default: 'UserDirectory/nwnx'.</param>
    /// <return>true if exported successfully, false if not.</return>
    public static bool ExportGIT(this NwArea area, string fileName = "", bool exportVariables = true, bool exportUUID = true, ObjectTypes objectTypeFilter = ObjectTypes.All, string alias = "NWNX") => AreaPlugin.ExportGIT(area, fileName, exportVariables.ToInt(), exportUUID.ToInt(), (int)objectTypeFilter, alias).ToBool();

    /// <summary>
    /// Export the .git file of this area to the UserDirectory/nwnx folder, or to the location of alias.
    /// </summary>
    /// <param name="area">The NwArea to check.</param>
    /// <param name="fileName">The filename, 16 characters or less and should be lowercase. If left blank the resref of oArea will be used.</param>
    /// <param name="newName">Optional new name of the area. Leave blank to use the current name.</param>
    /// <param name="newTag">Optional new tag of the area. Leave blank to use the current tag.</param>
    /// <param name="alias">The alias of the resource directory to add the .git file to. Default: 'UserDirectory/nwnx'.</param>
    /// <return>true if exported successfully, false if not.</return>
    public static bool ExportARE(this NwArea area, string fileName = "", string newName = "", string newTag = "", string alias = "NWNX") => AreaPlugin.ExportARE(area, fileName, newName, newTag, alias).ToBool();
  }
}
