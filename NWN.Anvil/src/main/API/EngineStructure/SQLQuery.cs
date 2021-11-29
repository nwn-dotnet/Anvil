using System;
using System.Collections.Generic;
using System.Numerics;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// A SQL Query.
  /// </summary>
  public sealed class SQLQuery : EngineStructure
  {
    private bool executed;
    private bool hasResult;
    internal SQLQuery(IntPtr handle) : base(handle) {}

    /// <summary>
    /// Returns "" if the last Sql command succeeded; or a human-readable error otherwise.
    /// </summary>
    public string Error
    {
      get
      {
        AssertQueryExecuted(true);
        return NWScript.SqlGetError(this);
      }
    }

    /// <summary>
    /// Gets the result of this query.<br/>
    /// NOTE: If <see cref="Results"/> have been enumerated, this will be the last enumerated value.
    /// </summary>
    public SQLResult Result
    {
      get
      {
        AssertQueryExecuted(true);
        return hasResult ? new SQLResult(this) : null;
      }
    }

    /// <summary>
    /// Gets the results of this query.<br/>
    /// NOTE: Results can only be enumerated once. Be careful with usage of LINQ extensions and loops.
    /// </summary>
    public IEnumerable<SQLResult> Results
    {
      get
      {
        AssertQueryExecuted(true);
        if (!hasResult)
        {
          yield break;
        }

        SQLResult result = new SQLResult(this);

        do
        {
          yield return result;
        }
        while (NWScript.SqlStep(this).ToBool());
      }
    }

    protected override int StructureId
    {
      get => NWScript.ENGINE_STRUCTURE_SQLQUERY;
    }

    public static implicit operator SQLQuery(IntPtr intPtr)
    {
      return new SQLQuery(intPtr);
    }

    /// <summary>
    /// Binds the specified parameter with the specified value.
    /// </summary>
    /// <param name="param">The parameter name to bind.</param>
    /// <param name="value">The value to bind to the parameter.</param>
    public void BindParam(string param, int value)
    {
      AssertQueryExecuted(false);
      NWScript.SqlBindInt(this, param, value);
    }

    /// <summary>
    /// Binds the specified parameter with the specified value.
    /// </summary>
    /// <param name="param">The parameter name to bind.</param>
    /// <param name="value">The value to bind to the parameter.</param>
    public void BindParam(string param, float value)
    {
      AssertQueryExecuted(false);
      NWScript.SqlBindFloat(this, param, value);
    }

    /// <summary>
    /// Binds the specified parameter with the specified value.
    /// </summary>
    /// <param name="param">The parameter name to bind.</param>
    /// <param name="value">The value to bind to the parameter.</param>
    public void BindParam(string param, string value)
    {
      AssertQueryExecuted(false);
      NWScript.SqlBindString(this, param, value);
    }

    /// <summary>
    /// Binds the specified parameter with the specified value.
    /// </summary>
    /// <param name="param">The parameter name to bind.</param>
    /// <param name="value">The value to bind to the parameter.</param>
    public void BindParam(string param, Vector3 value)
    {
      AssertQueryExecuted(false);
      NWScript.SqlBindVector(this, param, value);
    }

    /// <summary>
    /// Binds the specified parameter with the specified value.
    /// </summary>
    /// <param name="param">The parameter name to bind.</param>
    /// <param name="value">The value to bind to the parameter.</param>
    public void BindParam(string param, NwObject value)
    {
      AssertQueryExecuted(false);
      NWScript.SqlBindObject(this, param, value);
    }

    /// <summary>
    /// Executes this query.
    /// </summary>
    public void Execute()
    {
      AssertQueryExecuted(false);
      hasResult = NWScript.SqlStep(this).ToBool();
      executed = true;
    }

    private void AssertQueryExecuted(bool expected)
    {
      if (executed != expected)
      {
        string message = expected ? "The SQL query must be executed first." : "The SQL query has already been executed.";
        throw new InvalidOperationException(message);
      }
    }
  }
}
