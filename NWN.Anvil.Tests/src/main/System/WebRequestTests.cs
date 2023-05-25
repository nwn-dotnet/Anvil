using System.Net.Http;
using NUnit.Framework;

namespace Anvil.Tests.System
{
  [TestFixture(Category = "TestRunner")]
  public sealed class WebRequestTests
  {
    [Test]
    [TestCase("http://example.com")]
    [TestCase("https://example.com")]
    public void InvokeWebRequestIsSuccessful(string uri)
    {
      using HttpClient httpClient = new HttpClient();
      HttpResponseMessage response = httpClient.Send(new HttpRequestMessage(HttpMethod.Get, uri));

      Assert.That(response.IsSuccessStatusCode, Is.True, $"'{uri}' return a non-success code: {response.StatusCode}");
    }
  }
}
