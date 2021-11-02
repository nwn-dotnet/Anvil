namespace Anvil.API
{
  public enum GffResourceFieldType : uint
  {
    Byte = 0x0,
    Char = 0x1,
    Word = 0x2,
    Short = 0x3,
    DWord = 0x4,
    Int = 0x5,
    DWord64 = 0x6,
    Int64 = 0x7,
    Float = 0x8,
    Double = 0x9,
    CExoString = 0xA,
    CResRef = 0xB,
    CExoLocString = 0xC,
    Void = 0xD,
    Struct = 0xE,
    List = 0xF,
  }
}
