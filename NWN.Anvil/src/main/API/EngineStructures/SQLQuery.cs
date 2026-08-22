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
    internal SQLQuery(IntPtr handle, bool memoryOwn) : base(handle, memoryOwn) {}

    /// <summary>
    /// Returns an empty string if the last SQL command succeeded; or a human-readable error otherwise.<br/>
    /// Additionally, all SQL errors are sent to all connected players.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the query has not been executed.</exception>
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
    /// <exception cref="InvalidOperationException">Thrown if the query has not been executed.</exception>
    public SQLResult? Result
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
    /// <exception cref="InvalidOperationException">Thrown if the query has not been executed.</exception>
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

    /// <summary>
    /// Gets the name of the columns declared as a part of this query.
    /// </summary>
    public string[] Columns
    {
      get
      {
        string[] columns = new string[NWScript.SqlGetColumnCount(this)];
        for (int i = 0; i < columns.Length; i++)
        {
          columns[i] = NWScript.SqlGetColumnName(this, i);
        }

        return columns;
      }
    }

    private protected override int StructureId => NWScript.ENGINE_STRUCTURE_SQLQUERY;

    /// <summary>
    /// Converts a native pointer to a <see cref="SQLQuery"/> engine structure.
    /// </summary>
    /// <param name="intPtr">The native pointer to the SQL query.</param>
    /// <returns>A <see cref="SQLQuery"/> wrapping the specified pointer.</returns>
    public static implicit operator SQLQuery(IntPtr intPtr)
    {
      return new SQLQuery(intPtr, true);
    }

    /// <summary>
    /// Binds the specified parameter with the specified value.
    /// </summary>
    /// <param name="param">The parameter name to bind.</param>
    /// <param name="value">The value to bind to the parameter.</param>
    /// <exception cref="InvalidOperationException">Thrown if the query has already been executed.</exception>
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
    /// <exception cref="InvalidOperationException">Thrown if the query has already been executed.</exception>
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
    /// <exception cref="InvalidOperationException">Thrown if the query has already been executed.</exception>
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
    /// <exception cref="InvalidOperationException">Thrown if the query has already been executed.</exception>
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
    /// <exception cref="InvalidOperationException">Thrown if the query has already been executed.</exception>
    public void BindParam(string param, NwObject value)
    {
      AssertQueryExecuted(false);
      NWScript.SqlBindObject(this, param, value);
    }

    /// <summary>
    /// Executes this query.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the query has already been executed.</exception>
    public void Execute()
    {
      AssertQueryExecuted(false);
      hasResult = NWScript.SqlStep(this).ToBool();
      executed = true;
    }

    /// <summary>
    /// Reset this sqlquery, readying it for re-execution after results have been fetched.<br/>
    /// Existing BindParam values are kept, unless the clearBinds argument is set to true.
    /// </summary>
    /// <remarks>
    /// This command only works on successfully-prepared queries that have not errored out.
    /// </remarks>
    /// <param name="clearBinds">True if existing bind parameters should be cleared, false if they should be kept.</param>
    /// <exception cref="InvalidOperationException">Thrown if the query has not been executed.</exception>
    public void Reset(bool clearBinds = false)
    {
      AssertQueryExecuted(true);
      NWScript.SqlResetQuery(this, clearBinds.ToInt());
      executed = false;
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
