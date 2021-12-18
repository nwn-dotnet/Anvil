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

    public NwFeat Feat
    {
      get => NwFeat.FromFeatId(classFeat.nFeat);
    }

    public sbyte LevelGranted
    {
      get => classFeat.nLevelGranted.AsSByte();
    }

    public ClassFeatListType ListType
    {
      get => (ClassFeatListType)classFeat.nListType;
    }

    public bool OnMenu
    {
      get => classFeat.nOnClassRadial.ToBool();
    }
  }
}
