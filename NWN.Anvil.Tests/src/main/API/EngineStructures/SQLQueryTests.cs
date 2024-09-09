using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public class SQLQueryTests
  {
    [Test(Description = "Creating a SQL query and disposing the query explicitly frees the associated memory.")]
    public void CreateAndDisposeSQLQueryFreesNativeStructure()
    {
      SQLQuery query = NwModule.Instance.PrepareSQLQuery("SELECT * FROM 'test'");
      Assert.That(query.IsValid, Is.True, "SQL query was not valid after creation.");
      query.Dispose();
      Assert.That(query.IsValid, Is.False, "SQL query was still valid after disposing.");
    }
  }
}
