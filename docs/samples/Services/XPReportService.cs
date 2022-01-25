/*
 * Define a implementation to deserialize "xptable.2da", and use this class to determine how much XP is remaining until the next level.
 */

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace NWN.Anvil.Samples
{
  // This is the deserialization class for this specific type of 2da.
  // We can implement our own helper functions here that operate on the 2da data, and cache it.
  public sealed class ExpTable : ITwoDimArray
  {
    private readonly List<Entry> entries = new List<Entry>();

    /// <summary>
    /// Gets the max possible player level.
    /// </summary>
    public int MaxLevel => entries[^1].Level;

    /// <summary>
    /// Gets the amount of XP needed for the specified level.
    /// </summary>
    /// <param name="level">The level to lookup.</param>
    public int GetXpForLevel(int level)
    {
      return entries.First(entry => entry.Level == level).XP;
    }

    /// <summary>
    /// Gets the current level for a player with the specified XP.
    /// </summary>
    /// <param name="xp">The amount of xp.</param>
    public int GetLevelFromXp(int xp)
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

    void ITwoDimArray.DeserializeRow(int rowIndex, TwoDimEntry twoDimEntry)
    {
      // Use twoDimEntry(columnName) to get your serialized data, then convert it here.
      int level = int.Parse(twoDimEntry("Level"));
      uint xp = ParseXpColumn(twoDimEntry("XP"));

      if (xp > int.MaxValue)
      {
        return;
      }

      entries.Add(new Entry(level, (int)xp));
    }

    private uint ParseXpColumn(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        return 0;
      }

      return uint.TryParse(value, out uint retVal) ? retVal : uint.Parse(value);
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

  [ServiceBinding(typeof(XPReportService))]
  public class XPReportService
  {
    private readonly ExpTable expTable;

    public XPReportService(TwoDimArrayFactory twoDimArrayFactory)
    {
      expTable = twoDimArrayFactory.Get2DA<ExpTable>("exptable");
      NwModule.Instance.OnClientEnter += OnClientEnter;
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter onClientEnter)
    {
      NwPlayer player = onClientEnter.Player;
      int nextLevel = expTable.GetLevelFromXp(player.ControlledCreature.Xp) + 1;
      if (nextLevel > expTable.MaxLevel)
      {
        return;
      }

      player.SendServerMessage($"Next level up: {expTable.GetXpForLevel(nextLevel) - player.ControlledCreature.Xp}");
    }
  }
}
