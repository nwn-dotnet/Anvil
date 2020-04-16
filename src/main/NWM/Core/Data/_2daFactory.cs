using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN;
using NWNX;

namespace NWM.Core
{
  [Service]
  public class _2daFactory
  {
    private readonly Dictionary<string, I2da> cache = new Dictionary<string, I2da>();

    public T Get2DA<T>(string name, bool forceRefresh = false) where T : I2da, new()
    {
      if (!forceRefresh && cache.TryGetValue(name, out I2da value))
      {
        return (T) value;
      }

      return Load2DAToCache<T>(name);
    }

    private T Load2DAToCache<T>(string name) where T : I2da, new()
    {
      _2daAttribute info = typeof(T).GetCustomAttribute<_2daAttribute>();
      if (info == null)
      {
        throw new InvalidOperationException($"2DA Type {typeof(T)} does not define a _2da info attribute!");
      }

      T new2da = new T();
      for (int i = 0; i < UtilPlugin.Get2DARowCount(name); i++)
      {
        int rowIndex = i;
        new2da.DeserializeRow(info.Columns.Select(column => NWScript.Get2DAString(name, column, rowIndex)));
      }

      cache[name] = new2da;
      return new2da;
    }
  }
}