using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace TinyHttpClientPoolLib
{
    public class TinyHttpClientPool
    {
        private static TinyHttpClientPool _instance;

        public List<TinyHttpClient> _pool;

        public TinyHttpClientPool()
        {
            Internal_Initialize();
        }

        private void Internal_Initialize()
        {
            _pool = new List<TinyHttpClient>();
        }

        public HttpClient Fetch()
        {
            var client = _pool.FirstOrDefault(x => x.InUse == false);
                              

            if (client == null)
            {
                // No available clients, create a new one
                client = new TinyHttpClient();
                client.InUse = true;
                client.OnDispose += (sender, e) => client.InUse = false;
                _pool.Add(client);
                return client;
            }

            return _pool.FirstOrDefault();
        }

        public static void Initialize()
        {
            _instance = new TinyHttpClientPool();
        }
    }

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
