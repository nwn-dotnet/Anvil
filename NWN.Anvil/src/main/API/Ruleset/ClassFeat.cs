using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ClassFeat(CNWClass_Feat classFeat)
  {
    public NwFeat Feat => NwFeat.FromFeatId(classFeat.nFeat)!;

    public sbyte LevelGranted => classFeat.nLevelGranted.AsSByte();

    public ClassFeatListTypes ListTypes => (ClassFeatListTypes)classFeat.nListType;

    public bool OnMenu => classFeat.nOnClassRadial.ToBool();
  }
}
