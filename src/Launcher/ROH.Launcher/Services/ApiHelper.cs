using System.Net.Http;
using System.Threading.Tasks;
using ROH.Utils.ApiConfiguration;

namespace ROH.Launcher.Services
{
    // small helper if we need to perform Gateway calls with HttpClientFactory
    public class ApiHelper
    {
        readonly IHttpClientFactory _factory;

        public ApiHelper(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public HttpClient Create() => _factory.CreateClient("GatewayClient");
    }
}
