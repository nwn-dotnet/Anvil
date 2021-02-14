using System;
using System.Numerics;

namespace NWN.API
{
  /// <summary>
  /// Common math functions and constants.
  /// </summary>
  public static class NwMath
  {
    /// <summary>
    /// Constant value for converting feet to meters.
    /// </summary>
    /// <example>
    /// <c>float meters = 100f * FeetToMeters;</c>.
    /// </example>
    public const float FeetToMeters = 0.3048f;

    /// <summary>
    /// Constant value for converting meters to feet.
    /// </summary>
    /// <example>
    /// <c>float feet = 100f * MetersToFeet;</c>.
    /// </example>
    public const float MetersToFeet = 1f / FeetToMeters;

    /// <summary>
    /// Constant value for converting yards to meters.
    /// </summary>
    /// <example>
    /// <c>float meters = 100f * YardsToMeters;</c>.
    /// </example>
    public const float YardsToMeters = 0.9144f;

    /// <summary>
    /// Constant value for converting meters to yards.
    /// </summary>
    /// <example>
    /// <c>float yards = 100f * MetersToYards;</c>.
    /// </example>
    public const float MetersToYards = 1f / YardsToMeters;

    /// <summary>
    /// Constant value for converting degrees to radians.
    /// </summary>
    /// <example>
    /// <c>float radians = 180f * DegToRad;</c>.
    /// </example>
    public const float DegToRad = (float) (Math.PI * 2f / 360f);

    /// <summary>
    /// Constant value for converting radians to degrees.
    /// </summary>
    /// <example>
    /// <c>float degrees = 1.5708f * RadToDeg;</c>.
    /// </example>
    public const float RadToDeg = 1f / DegToRad;

    /// <inheritdoc cref="AngleToVector3"/>
    public static Vector2 AngleToVector2(float angle)
    {
      float radians = angle * DegToRad;
      return new Vector2((float) Math.Cos(radians), (float) Math.Sin(radians));
    }

    /// <summary>
    /// Converts the specified angle into a direction/heading vector.
    /// </summary>
    /// <param name="angle">The angle to convert.</param>
    /// <returns>A normalised direction vector for the specified angle.</returns>
    public static Vector3 AngleToVector3(float angle)
    {
      float radians = angle * DegToRad;
      return new Vector3((float) Math.Cos(radians), (float) Math.Sin(radians), 0f);
    }

    /// <inheritdoc cref="VectorToAngle(System.Numerics.Vector3)"/>
    public static float VectorToAngle(Vector2 direction)
    {
      return (float) (Math.Atan2(direction.Y, direction.X) * RadToDeg);
    }

    /// <summary>
    /// Converts the specified direction/heading vector to an angle.
    /// </summary>
    /// <param name="direction">The direction/heading vector to convert.</param>
    /// <returns>The angle, in degrees matching the direction of this vector.</returns>
    public static float VectorToAngle(Vector3 direction)
    {
      return (float) (Math.Atan2(direction.Y, direction.X) * RadToDeg);
    }
  }
}
