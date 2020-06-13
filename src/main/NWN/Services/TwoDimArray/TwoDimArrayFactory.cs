using System.Collections.Generic;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWN.Services
{
  [ServiceBinding(typeof(TwoDimArrayFactory))]
  public sealed class TwoDimArrayFactory
  {
    private readonly Dictionary<string, ITwoDimArray> cache = new Dictionary<string, ITwoDimArray>();

    public T Get2DA<T>(string name, bool forceRefresh = false) where T : ITwoDimArray, new()
    {
      name = name.Replace(".2da", "");

      if (!forceRefresh && cache.TryGetValue(name, out ITwoDimArray value))
      {
        return (T) value;
      }

      return Load2DAToCache<T>(name);
    }

    private T Load2DAToCache<T>(string name) where T : ITwoDimArray, new()
    {
      T new2da = new T();
      for (int i = 0; i < UtilPlugin.Get2DARowCount(name); i++)
      {
        int rowIndex = i;
        new2da.DeserializeRow(rowIndex, (column) => NWScript.Get2DAString(name, column, rowIndex));
      }

      cache[name] = new2da;
      return new2da;
    }
  }
}