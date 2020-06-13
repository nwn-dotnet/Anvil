using System;

namespace NWN.API
{
  public static class RandomExtensions
  {
    public static double Range(this Random random, double minValue, double maxValue)
    {
      double next = random.NextDouble();
      return minValue + next * (maxValue - minValue);
    }
  }
}