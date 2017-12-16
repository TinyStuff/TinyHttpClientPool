using System.Net.Http;

namespace TinyHttpClientPoolLib
{
    public interface ITinyHttpClientPool
    {
        HttpClient Fetch();
    }
}