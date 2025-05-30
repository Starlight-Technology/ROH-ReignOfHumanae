//-----------------------------------------------------------------------
// <copyright file="VersionServiceImplementation.cs" company="">
//     Author:  
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Grpc.Core;

using ROH.Service.Version.Interface;
using ROH.Utils.Helpers;

using System.Net;

using VersionServiceApi;

namespace ROH.Api.Version.Services;

public class VersionServiceImplementation(IGameVersionService service) : GameVersionService.GameVersionServiceBase
{
    public override async Task<DefaultResponse> GetCurrentVersion(Empty request, ServerCallContext context)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);

            StandardModels.Response.DefaultResponse currentVersion = await service.GetCurrentVersionAsync()
                .ConfigureAwait(true);

            return new DefaultResponse
            {
                Message = currentVersion.Message,
                StatusCode = (int)currentVersion.HttpStatus,
                ObjectResponse = currentVersion.ObjectResponse.ToJson()
            };
        }
        catch (Exception ex)
        {
            return new DefaultResponse { Message = ex.Message, StatusCode = (int)HttpStatusCode.BadRequest };
        }
    }

    public override async Task<BooleanResponse> VerifyIfVersionExist(
        VersionServiceApi.Guid request,
        ServerCallContext context)
    {
        bool response = await service.VerifyIfVersionExistAsync(request.Guid_).ConfigureAwait(true);

        return new BooleanResponse { Result = response };
    }
}
