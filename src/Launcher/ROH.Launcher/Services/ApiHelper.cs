namespace ROH.Launcher.Services;

// small helper if we need to perform Gateway calls with HttpClientFactory
public class ApiHelper
{
    private readonly IHttpClientFactory _factory;

    public ApiHelper(IHttpClientFactory factory) => _factory = factory;

    public HttpClient Create() => _factory.CreateClient("GatewayClient");
}
