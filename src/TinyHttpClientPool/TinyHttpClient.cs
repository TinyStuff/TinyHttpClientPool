using System;
using System.Net.Http;

namespace TinyHttpClientPoolLib
{
    public class TinyHttpClient : HttpClient
    {
        public bool InUse { get; set; }
        public EventHandler OnDispose;

        protected override void Dispose(bool disposing)
        {
            OnDispose?.Invoke(this, new EventArgs());
        }
    }
}
