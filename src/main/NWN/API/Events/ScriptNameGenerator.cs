using System;
using System.Text;
using NWN.API.Constants;
using NWN.Core;
using NWN.Services;

namespace NWN.API.Events
{
  internal static class ScriptNameGenerator
  {
    private static readonly char[] scriptChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    private static readonly Random random = new Random();
    private static readonly StringBuilder stringBuilder = new StringBuilder(ScriptConstants.MaxScriptNameSize);

    public static string Create()
    {
      stringBuilder.Clear();
      for (int i = 0; i < ScriptConstants.MaxScriptNameSize; i++)
      {
        stringBuilder.Append(scriptChars[random.Next(scriptChars.Length)]);
      }

      return stringBuilder.ToString();
    }
  }
}