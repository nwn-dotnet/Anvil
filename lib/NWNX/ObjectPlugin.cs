namespace NWNX
{
  [NWNXPlugin(PLUGIN_NAME)]
  internal class ObjectPlugin
  {
    public const string PLUGIN_NAME = "NWNX_Object";

    // Serialize the full object (including locals, inventory, etc) to base64 string
    public static string Serialize(uint obj)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "Serialize");
      NWN.Internal.NativeFunctions.nwnxPushObject(obj);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.nwnxPopString();
    }

    // Deserialize the object. The object will be created outside of the world and
    // needs to be manually positioned at a location/inventory.
    public static uint Deserialize(string serialized)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "Deserialize");
      NWN.Internal.NativeFunctions.nwnxPushString(serialized);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.nwnxPopObject();
    }
  }
}