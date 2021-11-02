using System;
using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class GffResourceFieldList : GffResourceField, IReadOnlyList<GffResourceFieldStruct>
  {
    private readonly CResList list;

    public GffResourceFieldList(CResGFF resGff, CResList list, uint count) : base(resGff)
    {
      this.list = list;
      Count = (int)count;
    }

    public override GffResourceFieldType FieldType
    {
      get => GffResourceFieldType.List;
    }

    public int Count { get; }

    public GffResourceFieldStruct this[int index]
    {
      get
      {
        if (index >= Count)
        {
          throw new IndexOutOfRangeException("Index was outside the bounds of the list.");
        }

        CResStruct resStruct = new CResStruct();
        return ResGff.GetListElement(resStruct, list, (uint)index).ToBool() ? new GffResourceFieldStruct(ResGff, resStruct) : null;
      }
    }

    public override GffResourceField this[uint index]
    {
      get => this[(int)index];
    }

    public IEnumerator<GffResourceFieldStruct> GetEnumerator()
    {
      for (int i = 0; i < Count; i++)
      {
        yield return this[i];
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
