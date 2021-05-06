using NWN.API;
using NWN.API.Constants;
using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Util
  {
    static Util()
    {
      PluginUtils.AssertPluginExists<UtilPlugin>();
    }

    /// <summary>
    /// Gets the vaulte of customTokenNumber.
    /// </summary>
    /// <param name="customTokenNumber">The token number to query.</param>
    /// <returns>The string representation of the token value.</returns>
    public static string GetCustomToken(int customTokenNumber) => UtilPlugin.GetCustomToken(customTokenNumber);

    /// <summary>
    /// Compiles and adds a script to the UserDirectory/nwnx folder.<br/>
    /// Will override existing scripts that are in the module.
    /// </summary>
    /// <param name="fileName">The script filename without extension, 16 or less characters.</param>
    /// <param name="scriptData">The script data to compile.</param>
    /// <param name="wrapIntoMain">Set to TRUE to wrap sScriptData into void main(){}.</param>
    /// <returns>"" on success, or the compilation error.</returns>
    public static string AddScript(string fileName, string scriptData, bool wrapIntoMain = false) => UtilPlugin.AddScript(fileName, scriptData, wrapIntoMain.ToInt());

    /// <summary>
    /// Gets the contents of a .nss script file as a string.
    /// </summary>
    /// <param name="scriptName">The name of the script to get the contents of.</param>
    /// <param name="maxLength">The max length of the return string, -1 to get everything.</param>
    /// <returns>The script file contents or "" on error.</returns>
    public static string GetNSSContents(string scriptName, int maxLength = -1) => UtilPlugin.GetNSSContents(scriptName, maxLength);

    /// <summary>
    /// Adds a nss file to the UserDirectory/nwnx folder. Will override existing nss files that are in the module.
    /// </summary>
    /// <param name="fileName">The script filename without extension, 16 or less characters.</param>
    /// <param name="contents">The contents of the nss file.</param>
    /// <returns>true on success.</returns>
    public static int AddNSSFile(string fileName, string contents) => UtilPlugin.AddNSSFile(fileName, contents);

    /// <summary>
    /// Remove sFileName of nType from the UserDirectory/nwnx folder.
    /// </summary>
    /// <param name="fileName">The filename without extension, 16 or less characters.</param>
    /// <param name="refType">The ResRef type.</param>
    /// <returns>true on success.</returns>
    public static bool RemoveNWNXResourceFile(string fileName, ResRefType refType) => UtilPlugin.RemoveNWNXResourceFile(fileName, (int) refType).ToBool();

    /// <summary>
    /// Register a server console command that will execute a script chunk.
    /// </summary>
    /// <param name="command">The name of the command.</param>
    /// <param name="scriptChunk">The script chunk to run. You can use $args to get the console command arguments.</param>
    /// <returns>true on success.</returns>
    public static bool RegisterServerConsoleCommand(string command, string scriptChunk) => UtilPlugin.RegisterServerConsoleCommand(command, scriptChunk).ToBool();

    /// <summary>
    /// Unregister a server console command that was registered with <see cref="RegisterServerConsoleCommand"/>.
    /// </summary>
    /// <param name="command">The name of the command.</param>
    public static void UnregisterServerConsoleCommand(string command) => UtilPlugin.UnregisterServerConsoleCommand(command);

    /// <summary>
    /// Determines if the given plugin exists and is enabled.
    /// </summary>
    /// <param name="plugin">The name of the plugin to check. This is the case sensitive plugin name as used by NWNX_CallFunction, NWNX_PushArgumentX (e.g. "NWNX_Creature").</param>
    /// <returns>true if the plugin exists and is enabled, otherwise false.</returns>
    public static bool PluginExists(string plugin) => UtilPlugin.PluginExists(plugin).ToBool();

    /// <summary>
    /// Gets the return value of the last run script with a StartingConditional.
    /// </summary>
#pragma warning disable SA1623 // Property summary documentation should match accessors
    public static bool ScriptReturnValue => UtilPlugin.GetScriptReturnValue().ToBool();
#pragma warning restore SA1623 // Property summary documentation should match accessors
  }
}
