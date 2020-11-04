using NWN.API.Constants;

namespace NWN.API
{
  public class ClassInfo
  {
    public readonly ClassType Type;
    public readonly int Level;

    internal ClassInfo(ClassType type, int level)
    {
      this.Type = type;
      this.Level = level;
    }
  }
}
