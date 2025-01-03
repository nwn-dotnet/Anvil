using System.Text;

namespace Anvil.Tests.Plugins
{
  internal static class PluginTestUtils
  {
    private const string PluginInfo = "[assembly: Anvil.Plugins.PluginInfo(Isolated = true)]";

    public static string GenerateServiceClass(string serviceName, string[] imports, string[] bindings, string[] baseTypes, string implementation)
    {
      StringBuilder source = new StringBuilder();
      AppendImports(source, imports);
      source.AppendLine();
      source.AppendLine(PluginInfo);
      source.AppendLine();

      AppendClassDefinition(source, serviceName, bindings, baseTypes);

      source.AppendLine("{");
      source.Append(implementation);
      source.AppendLine();
      source.AppendLine("}");

      return source.ToString();
    }

    private static void AppendClassDefinition(StringBuilder source, string serviceName, string[] bindings, string[] baseTypes)
    {
      source.AppendLine($"[Anvil.Services.ServiceBinding(typeof({serviceName}))]");
      foreach (string binding in bindings)
      {
        source.AppendLine($"[Anvil.Services.ServiceBinding(typeof({binding}))]");
      }

      source.Append($"public class {serviceName}");
      if (baseTypes.Length > 0)
      {
        source.Append($" : {string.Join(", ", baseTypes)}");
      }

      source.AppendLine();
    }

    private static void AppendImports(StringBuilder source, string[]? imports)
    {
      if (imports == null)
      {
        return;
      }

      foreach (string import in imports)
      {
        source.AppendLine($"using {import};");
      }
    }
  }
}
