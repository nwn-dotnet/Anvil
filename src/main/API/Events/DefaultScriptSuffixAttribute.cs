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

  internal static class DefaultScriptSuffixAttributeExtensions
  {
    public static string GetDefaultScriptSuffix(this Enum value)
    {
      FieldInfo enumVal = value.GetType().GetField(value.ToString());
      return enumVal.GetCustomAttribute<DefaultScriptSuffixAttribute>()?.ScriptSuffix ?? value.ToString().Substring(0, 4).ToLower();
    }
  }
}