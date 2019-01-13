using System;
using System.Threading.Tasks;

namespace ProxyApi.Providers
{
    public interface ICache
    {
        Task<T> Get<T>(string key, Func<T> addMethod);
    }
}
