using Grpc.Core;

using ROH.Service.Version.Interface;
using ROH.Utils.Helpers;

using System.Net;

using VersionServiceApi;

namespace ROH.Api.Version.Services;

public class VersionServiceImplementation(IGameVersionService service) : VersionServiceApi.GameVersionService.GameVersionServiceBase
{
    public async override Task<VersionServiceApi.DefaultResponse> GetCurrentVersion(Empty request, ServerCallContext context)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);

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

    public async override Task<BooleanResponse> VerifyIfVersionExist(VersionServiceApi.Guid request, ServerCallContext context)
    {
        var response = await service.VerifyIfVersionExistAsync(request.Guid_);

        return new BooleanResponse { Result = response };
    }
}
