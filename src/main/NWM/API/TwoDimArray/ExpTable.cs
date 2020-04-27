using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NWM.Core;

namespace NWM.API
{
  public class ExpTable : ITwoDimArray
  {
    private readonly List<Entry> entries = new List<Entry>();

    public int MaxLevel => entries[^1].Level;

    public int GetXpForLevel(int level)
    {
      return entries.First(entry => entry.Level == level).XP;
    }

    public int GetLevelFromXp(uint xp)
    {
      int level = 1;
      foreach (Entry entry in entries)
      {
        if (entry.XP > xp)
        {
          break;
        }

        level = entry.Level;
      }

      return level;
    }

    void ITwoDimArray.DeserializeRow(TwoDimEntry twoDimEntry)
    {
      int level = int.Parse(twoDimEntry("Level"));
      uint xp = ParseXpColumn(twoDimEntry("XP"));

      if (xp > int.MaxValue)
      {
        return;
      }

      entries.Add(new Entry(level, (int) xp));
    }

    private uint ParseXpColumn(string value)
    {
      return uint.TryParse(value, out uint retVal) ? retVal : uint.Parse(value.Substring(2), NumberStyles.AllowHexSpecifier);
    }

    private readonly struct Entry
    {
      public readonly int Level;
      public readonly int XP;

      public Entry(int level, int xp)
      {
        Level = level;
        XP = xp;
      }
    }
  }
}