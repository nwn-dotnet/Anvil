using System;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// Represents a json engine structure.
  /// </summary>
  /// <remarks>This class is specifically for compatibility with the base game.<br/>
  /// It is recommended to use <see cref="JsonUtility"/> and <see cref="System.Text.Json"/> for all json-related problems in C#.</remarks>
  /// <seealso cref="JsonUtility"/>
  public sealed class Json : EngineStructure
  {
    internal Json(IntPtr handle, bool memoryOwn) : base(handle, memoryOwn) {}

    private protected override int StructureId => NWScript.ENGINE_STRUCTURE_JSON;

    /// <summary>
    /// Converts a native pointer to a <see cref="Json"/> engine structure.
    /// </summary>
    /// <param name="intPtr">The native pointer to the JSON engine structure.</param>
    /// <returns>A <see cref="Json"/> wrapping the specified pointer.</returns>
    public static implicit operator Json(IntPtr intPtr)
    {
      return new Json(intPtr, true);
    }

    /// <summary>
    /// Parses the specified string as a json structure.
    /// </summary>
    /// <param name="jsonString">The json string to parse.</param>
    /// <returns>The parsed json structure.</returns>
    public static Json Parse(string jsonString)
    {
      return NWScript.JsonParse(jsonString);
    }

    /// <summary>
    /// Dumps the content of the json object to a string.
    /// </summary>
    /// <returns>The JSON string representation of this structure.</returns>
    public string Dump()
    {
      return NWScript.JsonDump(this);
    }

    /// <summary>
    /// Attempts to parse this json structure into a <see cref="NwObject"/>.
    /// </summary>
    /// <param name="location">The location to spawn the parsed object.</param>
    /// <param name="owner">If this object is an item, the owner of the parsed item.</param>
    /// <param name="loadObjectState">If the object should load object state info from the json structure.</param>
    /// <returns>The created object, or null if parsing failed.</returns>
    public NwObject? ToNwObject(Location location, NwGameObject? owner = null, bool loadObjectState = true)
    {
      return NWScript.JsonToObject(this, location, owner, loadObjectState.ToInt()).ToNwObject();
    }

    /// <summary>
    /// Attempts to parse this json structure into a <see cref="NwObject"/>.
    /// </summary>
    /// <param name="location">The location to spawn the parsed object.</param>
    /// <param name="owner">If this object is an item, the owner of the parsed item.</param>
    /// <param name="loadObjectState">If the object should load object state info from the json structure.</param>
    /// <typeparam name="T">The expected NWN object type.</typeparam>
    /// <returns>The created object, or null if parsing failed.</returns>
    public T? ToNwObject<T>(Location location, NwGameObject? owner = null, bool loadObjectState = true) where T : NwObject
    {
      return NWScript.JsonToObject(this, location, owner, loadObjectState.ToInt()).ToNwObject<T>();
    }
  }
}
