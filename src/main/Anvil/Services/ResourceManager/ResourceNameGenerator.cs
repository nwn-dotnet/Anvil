using System;
using System.Text;
using NWN.API.Constants;

namespace Anvil.Services
{
  internal static class ResourceNameGenerator
  {
    private static readonly char[] ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    private static readonly Random Random = new Random();
    private static readonly StringBuilder StringBuilder = new StringBuilder(ScriptConstants.MaxScriptNameSize);

    public static string Create()
    {
      StringBuilder.Clear();
      for (int i = 0; i < ScriptConstants.MaxScriptNameSize; i++)
      {
        StringBuilder.Append(ValidChars[Random.Next(ValidChars.Length)]);
      }

      return StringBuilder.ToString();
    }
  }
}
