/*
 * The MIT License (MIT)
 * Copyright (c) 2017 Johan Karlsson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights 
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is furnished 
 * to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 * DEALINGS IN THE SOFTWARE.
 *
 */

using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System;

namespace TinyHttpClientPoolLib
{
    public class TinyHttpClientPool : ITinyHttpClientPool
    {
        private static TinyHttpClientPool _instance;
        private List<TinyHttpClient> _pool;

        public int AvailableCount => _pool.Where(x => x.State == State.Available).Count(); 
        public int TotalPoolSize => _pool.Where(x => x.State != State.Disposed).Count(); 

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
            if (_instance == null)
            {
                throw new Exception("You must call TinyHttpClientPool.Initialize if you plan to use the static methods");
            }

            return _instance.Fetch();
        }

        public HttpClient Fetch()
        {
            lock (_pool)
            {
                var client = _pool.FirstOrDefault(x => x.State == State.Available);
                                  
                if (client == null)
                {
                    // No available clients, create a new one
                    client = new TinyHttpClient();
                    client.State = State.InUse;
                    client.OnDispose += (sender, e) => client.State = State.Available;
                    _pool.Add(client);
                }

                return client;
            }
        }

        public void Flush()
        {
            lock (_pool)
            {
                var clientsToRemove = new List<HttpClient>();
                foreach (var client in _pool.Where(x => x.State == State.Available).ToList())
                {
                    client.State = State.Disposed;

                    // Cast to make sure we call the base dispose
                    var baseClient = client as HttpClient;
                    baseClient.Dispose();

                    _pool.Remove(client);
                }
            }
        }

        public static void Initialize()
        {
            _instance = new TinyHttpClientPool();
        }
    }
}
