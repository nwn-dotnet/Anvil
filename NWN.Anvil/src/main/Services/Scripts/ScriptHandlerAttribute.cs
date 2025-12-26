using System;
using JetBrains.Annotations;

namespace Anvil.Services
{
  /// <summary>
  /// Indicates that the method in the service should be used to handle all NWScript calls to the specified <see cref="ScriptName"/>.
  /// </summary>
  /// <example>[!code-csharp[](~/../NWN.Anvil.Samples/src/main/Services/BasicScriptHandler.cs)]</example>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  [MeansImplicitUse]
  public sealed class ScriptHandlerAttribute(string scriptName) : Attribute
  {
    public readonly string ScriptName = scriptName;
  }
}
