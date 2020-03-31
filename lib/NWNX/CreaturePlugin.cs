using NWM.API.Constants;
using NWN;

namespace NWNX
{
	[NWNXPlugin(PLUGIN_NAME)]
  public class CreaturePlugin
  {
	  private const string PLUGIN_NAME = "NWNX_Creature";

		// Gives the provided creature the provided feat.
		public static void AddFeat(uint creature, Feat feat)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "AddFeat");
			Internal.NativeFunctions.nwnxPushInt((int) feat);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
		}

		// Gives the provided creature the provided feat.
		// Adds the feat to the stat list at the provided level.
		public static void AddFeatByLevel(uint creature, Feat feat, int level)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "AddFeatByLevel");
			Internal.NativeFunctions.nwnxPushInt(level);
			Internal.NativeFunctions.nwnxPushInt((int) feat);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
		}

		// Removes from the provided creature the provided feat.
		public static void RemoveFeat(uint creature, Feat feat)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "RemoveFeat");
			Internal.NativeFunctions.nwnxPushInt((int) feat);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
		}
  }
}