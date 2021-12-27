using System;

namespace Anvil.TestRunner
{
  /// <summary>
  /// Instructs the test runner that this plugin contains nunit tests that should be run.
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly)]
  public sealed class ExecutePluginTestsAttribute : Attribute {}
}
