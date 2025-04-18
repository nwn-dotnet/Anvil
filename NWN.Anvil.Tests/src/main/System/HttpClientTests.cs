using System.Net.Http;
using NUnit.Framework;

namespace Anvil.Tests.System
{
  [TestFixture]
  public sealed class HttpClientTests
  {
    [Test]
    [TestCase("http://github.com")]
    [TestCase("https://github.com")]
    public void InvokeHttpRequestIsSuccessful(string uri)
    {
      using HttpClient httpClient = new HttpClient();
      HttpResponseMessage response = httpClient.Send(new HttpRequestMessage(HttpMethod.Get, uri));

      Assert.That(response.IsSuccessStatusCode, Is.True, $"'{uri}' return a non-success code: {response.StatusCode}");
    }
  }
}
