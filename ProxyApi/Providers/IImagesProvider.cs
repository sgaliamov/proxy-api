using System.Threading.Tasks;
using ProxyApi.Providers.Models;

namespace ProxyApi.Providers
{
    public interface IImagesProvider
    {
        Task<Response> Search(int page, string text, Box box = null);
        Task<Response> Info(string id);
    }

    public interface ICachedImagesProvider : IImagesProvider { }
}
