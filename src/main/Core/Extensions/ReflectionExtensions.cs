using System.Reflection;

namespace NWM.Core
{
  public static class ReflectionExtensions
  {
    public static string GetFullName(this MemberInfo member)
    {
      return member.DeclaringType != null ? $"{member.DeclaringType.FullName}.{member.Name}" : member.Name;
    }
  }
}