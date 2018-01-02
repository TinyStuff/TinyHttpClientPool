using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TinyHttpClientPoolLib.Tests
{
    public class DummyMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var ret = new Task<HttpResponseMessage>(() => {
                return new HttpResponseMessage(System.Net.HttpStatusCode.SeeOther);
            });
            ret.Start();
            return ret;
        }
    }
}
