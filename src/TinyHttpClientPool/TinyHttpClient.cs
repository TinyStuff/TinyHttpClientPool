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
