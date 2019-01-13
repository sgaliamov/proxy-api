using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProxyApi.Providers.Models
{
    public sealed class Response
    {
        public string ContentType { get; set; }
        public string Content { get; set; }
        public int StatusCode { get; set; }
    }
}
