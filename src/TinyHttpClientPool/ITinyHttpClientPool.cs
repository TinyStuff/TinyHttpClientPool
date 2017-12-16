using System.Net.Http;

namespace TinyHttpClientPoolLib
{
    public interface ITinyHttpClientPool
    {
        /// <summary>
        /// Fetches or creates a new instance of a HttpClient from the pool
        /// </summary>
        /// <returns>The fetch.</returns>
        HttpClient Fetch();

        /// <summary>
        /// Flushes the pool and disposes all HttpClients that are not in use
        /// </summary>
        void Flush();
    }
}
