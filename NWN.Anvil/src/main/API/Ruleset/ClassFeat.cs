using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ClassFeat
  {
    private readonly CNWClass_Feat classFeat;

    public ClassFeat(CNWClass_Feat classFeat)
    {
      this.classFeat = classFeat;
    }

    public NwFeat Feat => NwFeat.FromFeatId(classFeat.nFeat)!;

    public sbyte LevelGranted => classFeat.nLevelGranted.AsSByte();

    public ClassFeatListTypes ListTypes => (ClassFeatListTypes)classFeat.nListType;

    public bool OnMenu => classFeat.nOnClassRadial.ToBool();
  }
}
