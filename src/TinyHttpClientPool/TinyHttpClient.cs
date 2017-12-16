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

using System;
using System.Net.Http;

namespace TinyHttpClientPoolLib
{
    public class TinyHttpClient : HttpClient
    {
        public State State { get; set; }

        /// <summary>
        /// Raised when the internal HttpClient is disposed
        /// </summary>
        /// <remarks>
        /// Since the Dispose method is overridden the internal
        /// HttpClient is actually never disposed.
        /// </remarks>
        public EventHandler OnDispose;

        /// <summary>
        /// Overrides the dispose to return the client to the pool.
        /// </summary>
        /// <remarks>It doesn't dispose the underlying HttpClient</remarks>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            OnDispose?.Invoke(this, new EventArgs());
        }
    }
}
