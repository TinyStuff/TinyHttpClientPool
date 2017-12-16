using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System;

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

        public static HttpClient FetchClient()
        {
            if(_instance == null)
            {
                throw new Exception("You must call TinyHttpClientPool.Initialize if you plan to use the static methods");
            }

            return _instance.Fetch();
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
            }

            return client;
        }

        public static void Initialize()
        {
            _instance = new TinyHttpClientPool();
        }
    }
}
