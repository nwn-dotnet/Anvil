using System.Linq;

namespace Anvil.API
{
  public static class ExpTableExtensions
  {
    /// <summary>
    /// Gets the current level for a player with the specified XP.
    /// </summary>
    /// <param name="table">The table to lookup.</param>
    /// <param name="xp">The amount of xp.</param>
    public static int? GetLevelFromXP(this TwoDimArray<ExpTableEntry> table, int xp)
    {
      int? level = 1;
      foreach (ExpTableEntry entry in table)
      {
        if (entry.XP > xp)
        {
          break;
        }

        level = entry.Level;
      }

      return level;
    }

    /// <summary>
    /// Gets the amount of XP needed for the specified level.
    /// </summary>
    /// <param name="table">The table to lookup.</param>
    /// <param name="level">The level to lookup.</param>
    public static uint? GetXPFromLevel(this TwoDimArray<ExpTableEntry> table, int level)
    {
      return table.FirstOrDefault(entry => entry.Level == level)?.XP;
    }
  }
}
