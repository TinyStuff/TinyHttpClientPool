using System.Net.Http;

namespace TinyHttpClientPoolLib
{
    public interface ITinyHttpClientPool
    {
        /// <summary>
        /// Gets the count of HttpClients that are ready to be used
        /// </summary>
        /// <value>The available count</value>
        int AvailableCount { get; }

        /// <summary>
        /// Gets the total size of the pool excluding disposed but not cleared HttpClients
        /// </summary>
        /// <value>The total size of the pool</value>
        int TotalPoolSize { get; }

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
