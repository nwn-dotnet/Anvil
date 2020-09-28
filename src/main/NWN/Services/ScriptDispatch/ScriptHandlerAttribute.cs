using System;
using JetBrains.Annotations;

namespace NWN.Services
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  [MeansImplicitUse]
  public class ScriptHandlerAttribute : Attribute
  {
    public readonly string ScriptName;

    public ScriptHandlerAttribute(string scriptName)
    {
      ScriptName = scriptName;
    }
  }
}
