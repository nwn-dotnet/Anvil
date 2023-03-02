/*
 * Examples for creating and using Cassowary solvers.
 */

using Anvil.API;
using Anvil.Services;
using NLog;

namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(CassowaryExamples))]
  public class CassowaryExamples
  {
    // Gets the server log. By default, this reports to "anvil.log"
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Based on https://nwnlexicon.com/index.php?title=Cassowary & https://cassowary.readthedocs.io/en/latest/topics/theory.html
    public CassowaryExamples()
    {
      // Create an empty cassowary solver.
      Cassowary cTest = new Cassowary();

      // Add some constraints
      cTest.AddConstraint("middle == (left + right) / 2");
      cTest.AddConstraint("right == left + 10");
      cTest.AddConstraint("right <= 100");
      cTest.AddConstraint("left >= 0");

      // Results should be 90, 95 and 100
      Log.Info($"Solution 1: Left: {cTest.GetValue("left")}, Middle: {cTest.GetValue("middle")}, Right: {cTest.GetValue("right")}");

      // Suggest middle is 45
      cTest.SuggestValue("middle", 45f);

      // Results should now be 40, 45 and 50
      Log.Info($"Solution 2: Left: {cTest.GetValue("left")}, Middle: {cTest.GetValue("middle")}, Right: {cTest.GetValue("right")}");
    }
  }
}
