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
        private readonly List<TinyHttpClient> _pool;

        public static TinyHttpClientPool Current => _instance;

        public int AvailableCount => _pool.Where(x => x.State == State.Available).Count(); 
        public int TotalPoolSize => _pool.Where(x => x.State != State.Disposed).Count();

        public TinyHttpClientPoolConfiguration Configuration { get; set; }

        /// <summary>
        /// This Action will be called every time a new instance of 
        /// an HttpClient is created for the pool.
        /// </summary>
        /// <remarks>
        /// A good place to set base urls, common headers and so on
        /// </remarks>
        public Action<HttpClient> ClientInitialization;

        /// <summary>
        /// Occurs when pool changed. Mostly used for statistics and monitoring.
        /// </summary>
        /// <remarks>
        /// Raised when a HttpClient is created, fetched or disposed.
        /// </remarks>
        public event EventHandler PoolChanged;

        public TinyHttpClientPool(TinyHttpClientPoolConfiguration configuration = null)
        {
            _pool = new List<TinyHttpClient>();

            Configuration = configuration ?? new TinyHttpClientPoolConfiguration();
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

                    // Allow for user injected initialization of the client
                    ClientInitialization?.Invoke(client);

                    // Hook up events
                    client.OnDispose += (sender, e) =>
                    {
                        client.State = State.Available;

                        if (Configuration.ResetHeadersOnReuse)
                        {
                            client.DefaultRequestHeaders.Clear();
                        }

                        PoolChanged?.Invoke(this, new EventArgs());
                    };
                        
                    _pool.Add(client);
                }

                client.State = State.InUse;
				PoolChanged?.Invoke(this, new EventArgs());

                return client;
            }
        }

        public void Flush()
        {
            lock (_pool)
            {
                foreach (var client in _pool.Where(x => x.State == State.Available).ToList())
                {
                    client.State = State.Disposed;
                    client.InternalDispose();
                    _pool.Remove(client);
                }

                PoolChanged?.Invoke(this, new EventArgs());
            }
        }

        public static void Initialize()
        {
            if (_instance == null)
            {
                _instance = new TinyHttpClientPool();
            }
        }
    }
}
