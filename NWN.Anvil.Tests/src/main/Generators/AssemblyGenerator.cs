using System;
using System.IO;
using System.Linq;
using Anvil.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace Anvil.Tests.Generators
{
  internal sealed class AssemblyGenerator
  {
    public static void GenerateAssembly(Stream writeAssemblyStream, string assemblyName, string sourceCode)
    {
      CSharpCompilation compileJob = GenerateCode(assemblyName, sourceCode);
      EmitResult compileResult = compileJob.Emit(writeAssemblyStream);

      if (!compileResult.Success)
      {
        throw new Exception($"Compilation failed:\n{string.Join('\n', compileResult.Diagnostics)}");
      }
    }

    private static CSharpCompilation GenerateCode(string assemblyName, string sourceCode)
    {
      SourceText codeString = SourceText.From(sourceCode);
      CSharpParseOptions options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Default);

      SyntaxTree tree = CSharpSyntaxTree.ParseText(codeString, options);

      MetadataReference[] references =
      [
        ..Assemblies.RuntimeAssemblies.Select(assemblyPath => MetadataReference.CreateFromFile(assemblyPath)),
        MetadataReference.CreateFromFile(typeof(AnvilCore).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(AssemblyGenerator).Assembly.Location),
      ];

      return CSharpCompilation.Create(assemblyName, [tree], references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Debug, assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
    }
  }
}
