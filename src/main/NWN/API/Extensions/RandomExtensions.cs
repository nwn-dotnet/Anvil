using System;

namespace NWN.API
{
  /// <summary>
  /// Extensions to the Random class for reimplementing native random functions (Dice and Ranges).
  /// </summary>
  public static class RandomExtensions
  {
    /// <summary>
    /// Returns a random double in the specified range.
    /// </summary>
    /// <param name="random">The random instance.</param>
    /// <param name="minValue">The minimum value (inclusive)</param>
    /// <param name="maxValue">The maximum value (exclusive)</param>
    /// <returns>A random double in the specified range.</returns>
    public static double NextDouble(this Random random, double minValue, double maxValue)
    {
      double next = random.NextDouble();
      return minValue + next * (maxValue - minValue);
    }

    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to 0.0f, and less than 1.0f.
    /// </summary>
    /// <param name="random">The random instance.</param>
    /// <returns>A random floating-point number that is greater than or equal to 0.0f, and less than 1.0f.</returns>
    public static float NextFloat(this Random random) => (float) random.NextDouble();

    /// <summary>
    /// Returns a random float in the specified range.
    /// </summary>
    /// <param name="random">The random instance.</param>
    /// <param name="minValue">The minimum value (inclusive)</param>
    /// <param name="maxValue">The maximum value (exclusive)</param>
    /// <returns>A random float in the specified range.</returns>
    public static float NextFloat(this Random random, float minValue, float maxValue)
      => (float) random.NextDouble(minValue, maxValue);

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