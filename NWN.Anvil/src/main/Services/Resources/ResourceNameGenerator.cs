using System;
using System.Text;
using Anvil.API;

namespace Anvil.Services
{
  internal static class ResourceNameGenerator
  {
    private const string ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    private static readonly StringBuilder StringBuilder = new StringBuilder(ScriptConstants.MaxScriptNameSize);

    public static string Create()
    {
      StringBuilder.Clear();
      for (int i = 0; i < ScriptConstants.MaxScriptNameSize; i++)
      {
        StringBuilder.Append(ValidChars[Random.Shared.Next(ValidChars.Length)]);
      }

      return StringBuilder.ToString();
    }
  }
}
