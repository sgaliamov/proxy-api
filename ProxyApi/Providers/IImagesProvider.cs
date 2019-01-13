using System.Net.Http;
using System.Threading.Tasks;

namespace ProxyApi.Providers
{
    public interface IImagesProvider
    {
        Task<HttpResponseMessage> Search(int page, string text, Box box = null);
    }
}
