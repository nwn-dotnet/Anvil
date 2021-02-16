using System;
using System.Collections.Generic;
using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;
using NWNX.API.Constants;

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
    /// Gets the name of the currently executing script.<br/>
    /// If depth is > 0, it will return the name of the script that called this one via ExecuteScript().
    /// </summary>
    /// <param name="depth">depth to seek the executing script.</param>
    /// <returns>The name of the currently executing script.</returns>
    public static string GetCurrentScriptName(int depth = 0) => UtilPlugin.GetCurrentScriptName();

    /// <summary>
    /// Gets a string that contains all ascii characters at their position (e.g. 'A' at 65). The character at index 0 is a space.
    /// </summary>
    public static string AsciiTableString { get; } = UtilPlugin.GetAsciiTableString();

    /// <summary>
    /// Gets the vaulte of customTokenNumber.
    /// </summary>
    /// <param name="customTokenNumber">The token number to query.</param>
    /// <returns>The string representation of the token value.</returns>
    public static string GetCustomToken(int customTokenNumber) => UtilPlugin.GetCustomToken(customTokenNumber);

    /// <summary>
    /// Convert an effect type to an itemproperty type.
    /// </summary>
    /// <param name="effect">The effect to convert to an itemproperty.</param>
    /// <returns>The converted itemproperty.</returns>
    public static NWN.API.ItemProperty AsItemProperty(this NWN.API.Effect effect) => UtilPlugin.EffectToItemProperty(effect);

    /// <summary>
    /// Convert an itemproperty type to an effect type.
    /// </summary>
    /// <param name="ip">ip The itemproperty to convert to an effect.</param>
    /// <returns>The converted effect.</returns>
    public static NWN.API.Effect AsEffect(this NWN.API.ItemProperty ip) => UtilPlugin.ItemPropertyToEffect(ip);

    /// <summary>
    /// Determines if the supplied resref exists and is of the specified type.
    /// </summary>
    /// <param name="resRef">The resref to check.</param>
    /// <param name="type">The type of this resref.</param>
    /// <returns>true if the supplied resref exists and is of the specified type, otherwise false.</returns>
    public static int IsValidResRef(this string resRef, ResRefType type = ResRefType.Creature) => UtilPlugin.IsValidResRef(resRef);

    /// <summary>
    /// Gets or sets the module real life minutes per in game hour.
    /// </summary>
    [Obsolete("Use NWServer.WorldTimer.MinutesPerHour instead.")]
    public static int MinutesPerHour
    {
      get => UtilPlugin.GetMinutesPerHour();
      set => UtilPlugin.SetMinutesPerHour(value);
    }

    /// <summary>
    /// Encodes a string for usage in a URL.
    /// </summary>
    /// <param name="str">The string to encode for a URL.</param>
    /// <returns>The url encoded string.</returns>
    public static string EncodeForURL(this string str) => UtilPlugin.EncodeStringForURL(str);

    /// <summary>
    /// Gets the row count for a 2da.
    /// </summary>
    /// <param name="twoDimArray">The 2da to check (do not include the .2da).</param>
    /// <returns>The amount of rows in the 2da.</returns>
    public static int Get2DARowCount(string twoDimArray) => UtilPlugin.Get2DARowCount(twoDimArray);

    /// <summary>
    /// Gets all ResRefs for the given type and filter.
    /// </summary>
    /// <param name="type">A ResRef type.</param>
    /// <param name="regexFilter">Lets you filter out resrefs using a regexfilter. For example: nwnx_.\* gets you all scripts prefixed with nwnx_.</param>
    /// <param name="moduleResourcesOnly">If true, only bundled module resources will be returned.</param>
    /// <returns>Any matching resrefs, otherwise an empty enumeration.</returns>
    public static IEnumerable<string> GetResRefs(ResRefType type, string regexFilter = "", bool moduleResourcesOnly = true)
    {
      int refType = (int) type;
      for (string resRef = UtilPlugin.GetFirstResRef(refType, regexFilter, moduleResourcesOnly.ToInt());
        resRef != string.Empty;
        resRef = UtilPlugin.GetNextResRef())
      {
        yield return resRef;
      }
    }

    /// <summary>
    /// Gets the server's current tick rate.
    /// </summary>
    [Obsolete]
    public static int ServerTicksPerSecond
    {
      get => UtilPlugin.GetServerTicksPerSecond();
    }

    /// <summary>
    /// Gets the last created objects of type T.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>The last created objects.</returns>
    public static IEnumerable<T> GetLastCreatedObjects<T>() where T : NwObject
    {
      int objType = (int) NwObject.GetObjectType<T>();

      uint current;
      int i;
      for (i = 1, current = UtilPlugin.GetLastCreatedObject(objType, i); current != NWScript.OBJECT_INVALID; i++, current = UtilPlugin.GetLastCreatedObject(objType, i))
      {
        yield return current.ToNwObject<T>();
      }
    }

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
    /// Sets the NWScript instruction limit. -1 resets the value to default.
    /// </summary>
    [Obsolete("Use NwServer.InstructionLimit instead.")]
    public static int InstructionLimit
    {
      set => UtilPlugin.SetInstructionLimit(value);
    }

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
    /// Gets the absolute path of the server's home directory (-userDirectory).
    /// </summary>
    [Obsolete("Use NwServer.UserDirectory instead.")]
    public static string UserDirectory => UtilPlugin.GetUserDirectory();

    /// <summary>
    /// Gets the return value of the last run script with a StartingConditional.
    /// </summary>
#pragma warning disable SA1623 // Property summary documentation should match accessors
    public static bool ScriptReturnValue => UtilPlugin.GetScriptReturnValue().ToBool();
#pragma warning restore SA1623 // Property summary documentation should match accessors
  }
}
