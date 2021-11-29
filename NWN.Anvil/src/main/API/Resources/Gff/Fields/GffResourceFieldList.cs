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
    private readonly List<GffResourceFieldStruct> children = new List<GffResourceFieldStruct>();

    internal GffResourceFieldList(CResGFF resGff, CResList list, uint count) : base(resGff)
    {
      for (uint i = 0; i < count; i++)
      {
        CResStruct resStruct = new CResStruct();
        GffResourceFieldStruct childField = ResGff.GetListElement(resStruct, list, i).ToBool() ? new GffResourceFieldStruct(ResGff, resStruct) : null;
        children.Add(childField);
      }
    }

    public override int Count
    {
      get => children.Count;
    }

    public override GffResourceFieldType FieldType
    {
      get => GffResourceFieldType.List;
    }

    public override IEnumerable<GffResourceFieldStruct> Values
    {
      get => children;
    }

    public override GffResourceFieldStruct this[int index]
    {
      get => children[index];
    }

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
