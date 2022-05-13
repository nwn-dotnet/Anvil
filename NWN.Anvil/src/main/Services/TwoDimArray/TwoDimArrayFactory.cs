using System;
using System.Collections.Generic;
using Anvil.API;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.Services
{
  //! ## Examples
  //! @include XPReportService.cs

  /// <summary>
  /// Creates a deserialized 2da structure using a <see cref="ITwoDimArray"/> converter.
  /// </summary>
  [ServiceBinding(typeof(TwoDimArrayFactory))]
  [Obsolete("Create instances of Anvil.API.TwoDimArray instead.")]
  public sealed class TwoDimArrayFactory
  {
    private readonly Dictionary<string, ITwoDimArray> cache = new Dictionary<string, ITwoDimArray>();
    private readonly InjectionService injectionService;

    private readonly CTwoDimArrays twoDimArrays = NWNXLib.Rules().m_p2DArrays;

    public TwoDimArrayFactory(InjectionService injectionService)
    {
      this.injectionService = injectionService;
    }

    /// <summary>
    /// Deserializes the given 2da using the specified format.
    /// </summary>
    /// <param name="name">The name of the 2DA resource.</param>
    /// <param name="forceRefresh">If true, always reloads the 2DA instead of using a cached version.</param>
    /// <typeparam name="T">The <see cref="ITwoDimArray"/> type to use to deserialize.</typeparam>
    /// <returns>The deserialized 2DA.</returns>
    public T? Get2DA<T>(string name, bool forceRefresh = false) where T : ITwoDimArray, new()
    {
      name = name.Replace(".2da", string.Empty);

      if (!forceRefresh && cache.TryGetValue(name, out ITwoDimArray? value))
      {
        return (T)value;
      }

      return Load2DAToCache<T>(name);
    }

    private T? Load2DAToCache<T>(string name) where T : ITwoDimArray, new()
    {
      T new2da = injectionService.Inject(new T());
      C2DA? twoDimArray = twoDimArrays.GetCached2DA(name.ToExoString(), true.ToInt());

      if (twoDimArray == null)
      {
        return default;
      }

      for (int i = 0; i < twoDimArray.m_nNumRows; i++)
      {
        int rowIndex = i;
        new2da.DeserializeRow(rowIndex, (column) => NWScript.Get2DAString(name, column, rowIndex));
      }

      cache[name] = new2da;
      return new2da;
    }
  }
}
