using System;
using System.Net.Http;

namespace TinyHttpClientPoolLib
{
    public class TinyHttpClientPoolConfiguration
    {
        public string BaseUrl { get; set; }
        public bool ResetHeadersOnReuse { get; set; }
        public HttpMessageHandler MessageHandler { get; set; }
    }
}
