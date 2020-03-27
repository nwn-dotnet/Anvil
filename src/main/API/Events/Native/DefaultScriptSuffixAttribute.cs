using System;
using System.Reflection;

namespace NWM.API
{
  [AttributeUsage(AttributeTargets.Field)]
  internal class DefaultScriptSuffixAttribute : Attribute
  {
    public readonly string ScriptSuffix;

    public DefaultScriptSuffixAttribute(string scriptSuffix)
    {
      this.ScriptSuffix = scriptSuffix;
    }
  }
}