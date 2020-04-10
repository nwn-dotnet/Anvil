using NWN;

namespace NWNX
{
  [NWNXPlugin(PLUGIN_NAME)]
  internal class ObjectPlugin
  {
    public const string PLUGIN_NAME = "NWNX_Object";

    public static InternalObjectType GetInternalObjectType(uint objectId)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetInternalObjectType");
      Internal.NativeFunctions.nwnxPushObject(objectId);
      Internal.NativeFunctions.nwnxCallFunction();
      return (InternalObjectType) Internal.NativeFunctions.nwnxPopInt();
    }

    // Returns an object from the provided object ID.
    // This is the counterpart to ObjectToString.
    public static uint StringToObject(string id)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "StringToObject");
      Internal.NativeFunctions.nwnxPushString(id);
      Internal.NativeFunctions.nwnxCallFunction();
      return Internal.NativeFunctions.nwnxPopObject();
    }

    // Serialize the full object (including locals, inventory, etc) to base64 string
    public static string Serialize(uint obj)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "Serialize");
      Internal.NativeFunctions.nwnxPushObject(obj);
      Internal.NativeFunctions.nwnxCallFunction();
      return Internal.NativeFunctions.nwnxPopString();
    }

    // Deserialize the object. The object will be created outside of the world and
    // needs to be manually positioned at a location/inventory.
    public static uint Deserialize(string serialized)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "Deserialize");
      Internal.NativeFunctions.nwnxPushString(serialized);
      Internal.NativeFunctions.nwnxCallFunction();
      return Internal.NativeFunctions.nwnxPopObject();
    }

    // Get an object's string variable variableName.
    public static string GetString(uint obj, string variableName)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetString");
      Internal.NativeFunctions.nwnxPushString(variableName);
      Internal.NativeFunctions.nwnxPushObject(obj);
      Internal.NativeFunctions.nwnxCallFunction();
      return Internal.NativeFunctions.nwnxPopString();
    }

    // Set an object's string variable variableName to newValue. Toggle persistence with persist.
    public static void SetString(uint obj, string variableName, string newValue, bool persist)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetString");
      Internal.NativeFunctions.nwnxPushInt(persist ? 1 : 0);
      Internal.NativeFunctions.nwnxPushString(newValue);
      Internal.NativeFunctions.nwnxPushString(variableName);
      Internal.NativeFunctions.nwnxPushObject(obj);
      Internal.NativeFunctions.nwnxCallFunction();
    }
  }
}