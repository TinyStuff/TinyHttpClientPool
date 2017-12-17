using System;
namespace TinyHttpClientPoolLib
{
    public class TinyHttpClientPoolConfiguration
    {
        public string BaseUrl { get; set; }
        public bool ResetHeadersOnReuse { get; set; }
    }
}
