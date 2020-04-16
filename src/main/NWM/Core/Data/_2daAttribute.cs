using System;

namespace NWM.Core
{
  [AttributeUsage(AttributeTargets.Class)]
  public class _2daAttribute : Attribute
  {
    public readonly string[] Columns;

    public _2daAttribute(params string[] columns)
    {
      this.Columns = columns;
    }
  }
}