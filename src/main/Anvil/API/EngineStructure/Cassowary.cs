using System;
using NWN.Core;

namespace Anvil.API
{
  public sealed class Cassowary : EngineStructure
  {
    internal Cassowary(IntPtr handle) : base(handle) {}

    protected override int StructureId
    {
      get => NWScript.ENGINE_STRUCTURE_CASSOWARY;
    }

    public static implicit operator Cassowary(IntPtr intPtr)
    {
      return new Cassowary(intPtr);
    }

    /// <summary>
    /// Gets a printable debug state of this solver, which may help you debug complex systems.
    /// </summary>
    public string DebugState
    {
      get => NWScript.CassowaryDebug(this);
    }

    /// <summary>
    /// Adds a constraint to the system.<br/>
    /// You cannot multiply or divide variables and expressions with each other.<br/>
    /// Doing so will result in a error when attempting to add the constraint.<br/>
    /// (You can, of course, multiply or divide by constants).<br/>
    /// </summary>
    /// <param name="constraintExpression">The constraint expression. Needs to be a valid comparison equation, one of: &gt;=, ==, &lt;=.</param>
    /// <param name="strength">A value >= CASSOWARY_STRENGTH_WEAK &amp;&amp; &lt;= CASSOWARY_STRENGTH_REQUIRED.</param>
    public void AddConstraint(string constraintExpression, float strength = CassowaryStrength.Required)
    {
      string error = NWScript.CassowaryConstrain(this, constraintExpression, strength);

      if (!string.IsNullOrEmpty(error))
      {
        throw new CassowaryException(error);
      }
    }

    /// <summary>
    /// Suggests a value to the solver.
    /// </summary>
    /// <param name="varName">The variable to suggest a value.</param>
    /// <param name="value">The suggested value.</param>
    /// <param name="strength">A value >= CASSOWARY_STRENGTH_WEAK &amp;&amp; &lt;= CASSOWARY_STRENGTH_REQUIRED.</param>
    public void SuggestValue(string varName, float value, float strength = CassowaryStrength.Strong)
    {
      NWScript.CassowarySuggestValue(this, varName, value, strength);
    }

    /// <summary>
    /// Gets the value for the specified variable.
    /// </summary>
    /// <param name="varName">The variable to query.</param>
    /// <returns>The value of the specified variable, otherwise 0.0 on an error.</returns>
    public float GetValue(string varName)
    {
      return NWScript.CassowaryGetValue(this, varName);
    }

    /// <summary>
    /// Clear out this solver, removing all state, constraints and suggestions.
    /// </summary>
    public void Reset()
    {
      NWScript.CassowaryReset(this);
    }
  }
}
