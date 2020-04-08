using NWN;

namespace NWNX
{
	[NWNXPlugin(PLUGIN_NAME)]
  internal class CreaturePlugin
  {
	  private const string PLUGIN_NAME = "NWNX_Creature";

		public static void AddFeat(uint creature, int feat)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "AddFeat");
			Internal.NativeFunctions.nwnxPushInt(feat);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
		}

		public static void AddFeatByLevel(uint creature, int feat, int level)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "AddFeatByLevel");
			Internal.NativeFunctions.nwnxPushInt(level);
			Internal.NativeFunctions.nwnxPushInt(feat);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
		}

		public static int GetFeatLevel(uint creature, int feat)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetFeatGrantLevel");
			Internal.NativeFunctions.nwnxPushInt(feat);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
			return Internal.NativeFunctions.nwnxPopInt();
		}

		public static int GetKnowsFeat(uint creature, int feat)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetKnowsFeat");
			Internal.NativeFunctions.nwnxPushInt(feat);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
			return Internal.NativeFunctions.nwnxPopInt();
		}

		public static void RemoveFeat(uint creature, int feat)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "RemoveFeat");
			Internal.NativeFunctions.nwnxPushInt(feat);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
		}

		public static int GetMemorisedSpellCountByLevel(uint creature, int classId, int level)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetMemorisedSpellCountByLevel");
			Internal.NativeFunctions.nwnxPushInt(level);
			Internal.NativeFunctions.nwnxPushInt(classId);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
			return Internal.NativeFunctions.nwnxPopInt();
		}

		public static void ClearMemorisedSpell(uint creature, int classId, int level, int index)
		{
			Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ClearMemorisedSpell");
			Internal.NativeFunctions.nwnxPushInt(index);
			Internal.NativeFunctions.nwnxPushInt(level);
			Internal.NativeFunctions.nwnxPushInt(classId);
			Internal.NativeFunctions.nwnxPushObject(creature);
			Internal.NativeFunctions.nwnxCallFunction();
		}
  }
}