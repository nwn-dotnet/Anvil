using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A <see cref="GffResourceField"/> containing a list of <see cref="GffResourceFieldStruct"/> values.
  /// </summary>
  public sealed class GffResourceFieldList : GffResourceField, IReadOnlyList<GffResourceFieldStruct>
  {
    private readonly List<GffResourceFieldStruct> children = [];

    internal GffResourceFieldList(CResGFF resGff, CResList list, uint count) : base(resGff)
    {
      for (uint i = 0; i < count; i++)
      {
        CResStruct resStruct = new CResStruct();
        GffResourceFieldStruct? childField = ResGff.GetListElement(resStruct, list, i).ToBool() ? new GffResourceFieldStruct(ResGff, resStruct) : null;
        if (childField != null)
        {
          children.Add(childField);
        }
      }
    }

    public override int Count => children.Count;

    public override GffResourceFieldType FieldType => GffResourceFieldType.List;

    public override IEnumerable<GffResourceFieldStruct> Values => children;

    public override GffResourceFieldStruct this[int index] => children[index];

    public IEnumerator<GffResourceFieldStruct> GetEnumerator()
    {
      return children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
