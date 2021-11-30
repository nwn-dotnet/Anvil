using System;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// Functions for generating random values (numbers, names).
  /// </summary>
  public static class NwRandom
  {
    /// <summary>
    /// Generates a random name using the specified name table.
    /// </summary>
    public static string RandomName(NameTable name = NameTable.FirstGenericMale)
    {
      return NWScript.RandomName((int)name);
    }

    /// <summary>
    /// Returns the result of rolling an amount of n-sided dice.
    /// </summary>
    /// <param name="random">The random instance.</param>
    /// <param name="sides">The number of sides on each die.</param>
    /// <param name="amount">The amount of dice to roll.</param>
    public static int Roll(this Random random, int sides, int amount = 1)
    {
      int total = 0;
      int randMax = sides + 1;

      for (int i = 0; i < amount; i++)
      {
        total += random.Next(1, randMax);
      }

      return total;
    }
  }
}
