using System;
using System.Threading.Tasks;

namespace ProxyApi.Providers
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> addMethod = null);
    }
}
