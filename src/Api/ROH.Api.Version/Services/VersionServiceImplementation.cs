using ROH.Service.Version.Interface;
using ROH.Utils.Helpers;

using System.Net;

namespace ROH.Api.Version.Services;

public class VersionServiceImplementation(IGameVersionService service) : VersionServiceApi.GameVersionService.GameVersionServiceBase
{
    public async Task<VersionServiceApi.DefaultResponse> GetCurrentVersion()
    {
        try
        {
            var currentVersion = await service.GetCurrentVersionAsync();

            return new VersionServiceApi.DefaultResponse()
            {
                Message = currentVersion.Message,
                StatusCode = (int)currentVersion.HttpStatus,
                ObjectResponse = currentVersion.ObjectResponse?.ObjectToJson()
            };
        }
        catch (Exception ex)
        {
            return new VersionServiceApi.DefaultResponse()
            {
                Message = ex.Message,
                StatusCode = (int)HttpStatusCode.BadRequest

            };
        }
    }
}
