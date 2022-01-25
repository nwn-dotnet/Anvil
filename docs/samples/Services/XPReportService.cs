/*
 * Define a implementation to deserialize "xptable.2da", and use this class to determine how much XP is remaining until the next level.
 */

using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace NWN.Anvil.Samples
{
  // This is the deserialization class for this specific type of 2da.
  // We can implement our own helper functions here that operate on the 2da data, and cache it.
  public sealed class ExpTableEntry : ITwoDimArrayEntry
  {
    public int Level { get; private set; }

    public int XP { get; private set; }

    // RowIndex is already populated externally, and we do not need to assign it in InterpretEntry.
    public int RowIndex { get; init; }

    // InterpretEntry is where we populate our entry properties (Level & XP) with the correct data.
    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Level = entry.GetInt("Level").GetValueOrDefault(0);
      XP = entry.GetInt("XP").GetValueOrDefault(0);
    }
  }

  [ServiceBinding(typeof(XPReportService))]
  public class XPReportService
  {
    // The TwoDimArray is created here.
    // ExpTableEntry (the type above) is passed in as a type parameter to be used to create our row data from exptable.2da.
    private readonly TwoDimArray<ExpTableEntry> expTable = new TwoDimArray<ExpTableEntry>("exptable.2da");

    public XPReportService()
    {
      NwModule.Instance.OnClientEnter += OnClientEnter;
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter onClientEnter)
    {
      NwPlayer player = onClientEnter.Player;
      int nextLevel = GetLevelFromXp(player.ControlledCreature.Xp) + 1;
      if (nextLevel > MaxLevel)
      {
        return;
      }

      player.SendServerMessage($"Next level up: {GetXpForLevel(nextLevel) - player.ControlledCreature.Xp}");
    }

    /// <summary>
    /// Gets the max possible player level.
    /// </summary>
    public int MaxLevel => expTable[^1].Level;

    /// <summary>
    /// Gets the amount of XP needed for the specified level.
    /// </summary>
    /// <param name="level">The level to lookup.</param>
    public int GetXpForLevel(int level)
    {
      return expTable.First(entry => entry.Level == level).XP;
    }

    /// <summary>
    /// Gets the current level for a player with the specified XP.
    /// </summary>
    /// <param name="xp">The amount of xp.</param>
    public int GetLevelFromXp(int xp)
    {
      int level = 1;
      foreach (ExpTableEntry entry in expTable.Rows)
      {
        if (entry.XP > xp)
        {
          break;
        }

        level = entry.Level;
      }

      return level;
    }
  }
}
