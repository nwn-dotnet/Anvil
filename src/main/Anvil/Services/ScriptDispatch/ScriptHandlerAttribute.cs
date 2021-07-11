using System;
using JetBrains.Annotations;

namespace NWN.Services
{
  //! ## Examples
  //! @include BasicScriptHandler.cs

  /// <summary>
  /// Indicates that the method in the service should be used to handle all NWScript calls to the specified <see cref="ScriptName"/>.
  /// </summary>
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
