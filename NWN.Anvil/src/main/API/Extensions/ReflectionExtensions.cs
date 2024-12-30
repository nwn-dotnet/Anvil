using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Anvil.Services;

namespace Anvil.API
{
  public static class ReflectionExtensions
  {
    /// <summary>
    /// Gets all attributes of a specific type on the specified type.
    /// </summary>
    /// <param name="type">The type to get attributes from</param>
    /// <param name="inherit">true if attributes should be inherited from base types</param>
    /// <typeparam name="T">The type of attribute to get</typeparam>
    /// <returns>An array of attributes applied to this type, or an empty array if no attributes are found.</returns>
    public static T[] GetCustomAttributes<T>(this Type type, bool inherit = true) where T : Attribute
    {
      return (T[])type.GetCustomAttributes(typeof(T), inherit);
    }

    /// <summary>
    /// Gets all attributes of a specific type on the specified member.
    /// </summary>
    /// <param name="member">The member to get attributes from</param>
    /// <param name="inherit">true if attributes should be inherited from base types</param>
    /// <typeparam name="T">The type of attribute to get</typeparam>
    /// <returns>An array of attributes applied to this member, or an empty array if no attributes are found.</returns>
    public static T[] GetCustomAttributes<T>(this MemberInfo member, bool inherit = true) where T : Attribute
    {
      return (T[])member.GetCustomAttributes(typeof(T), inherit);
    }

    /// <summary>
    /// Gets the full name of the specified type member.
    /// </summary>
    /// <param name="member">The member to get the name of.</param>
    /// <returns>The full name of the member, prefixed with the namespace and enclosing type (if valid). If the member is not declared in a type, returns the member name.</returns>
    public static string GetFullName(this MemberInfo member)
    {
      return member.DeclaringType != null ? $"{member.DeclaringType.FullName}.{member.Name}" : member.Name;
    }

    public static T? SafeGetCustomAttribute<T>(this MemberInfo memberInfo, bool inherit = true)
    {
      // GetCustomAttribute(Type) or GetCustomAttribute<T>() will throw an exception on types with missing assembly references, as they navigate into the type.
      return memberInfo.GetCustomAttributes(inherit).OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets the internal service name of this type.<br/>
    /// This name is used for determining execution order of the service.
    /// </summary>
    /// <param name="type">The <see cref="System.Type"/> of the service to resolve the name of.</param>
    /// <returns>The internal service name.</returns>
    internal static string GetInternalServiceName(this Type type)
    {
      int bindingPriority = type.GetServicePriority() - (int)InternalBindingPriority.Highest;
      return bindingPriority.ToString("D6") + type.FullName;
    }

    internal static int GetServicePriority(this Type type)
    {
      ServiceBindingOptionsAttribute? options = type.GetCustomAttribute<ServiceBindingOptionsAttribute>();

      int bindingPriority = options?.Priority ?? (int)InternalBindingPriority.Default;
      bindingPriority = Math.Clamp(bindingPriority, (int)InternalBindingPriority.Highest, (int)InternalBindingPriority.Lowest);

      return bindingPriority;
    }

    internal static T? GetCustomAttributeFromMetadata<T>(this Assembly metadataAssembly) where T : Attribute, new()
    {
      Type attributeType = typeof(T);
      CustomAttributeData? attributeData = metadataAssembly.GetCustomAttributesData().FirstOrDefault(data => data.AttributeType.FullName == attributeType.FullName);
      if (attributeData == null)
      {
        return null;
      }

      T attribute = new T();
      foreach (CustomAttributeNamedArgument argument in attributeData.NamedArguments)
      {
        object? value;
        if (argument.TypedValue.Value is IReadOnlyList<CustomAttributeTypedArgument> collection)
        {
          Type elementType = Type.GetType(argument.TypedValue.ArgumentType.GetElementType()!.FullName!)!;
          Array array = Array.CreateInstance(elementType, collection.Count);

          for (int i = 0; i < array.Length; i++)
          {
            array.SetValue(collection[i].Value, i);
          }

          value = array;
        }
        else
        {
          value = argument.TypedValue.Value;
        }

        if (argument.IsField)
        {
          attributeType.GetField(argument.MemberName)?.SetValue(attribute, value);
        }
        else
        {
          attributeType.GetProperty(argument.MemberName)?.SetValue(attribute, value);
        }
      }

      return attribute;
    }
  }
}
