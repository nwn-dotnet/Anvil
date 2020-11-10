using System;
using System.Text;
using NWN.API.Constants;

namespace NWN.API.Events
{
  internal static class ScriptNameGenerator
  {
    private static readonly char[] ScriptChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    private static readonly Random Random = new Random();
    private static readonly StringBuilder StringBuilder = new StringBuilder(ScriptConstants.MaxScriptNameSize);

    public static string Create()
    {
      StringBuilder.Clear();
      for (int i = 0; i < ScriptConstants.MaxScriptNameSize; i++)
      {
        StringBuilder.Append(ScriptChars[Random.Next(ScriptChars.Length)]);
      }

      return StringBuilder.ToString();
    }
  }
}
