//-----------------------------------------------------------------------
// <copyright file="LogServiceImplementation.cs" company="">
//     Author:  
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Grpc.Core;

using LogServiceApi;

using ROH.Service.Log;

namespace ROH.Api.Log.Services;

public class LogServiceImplementation(ILogService service) : LogServiceApi.LogService.LogServiceBase
{
    public override async Task<LogResponse> Log(LogRequest request, ServerCallContext context)
    {
        try
        {
            await service.LogException(request.Message).ConfigureAwait(true);
            return new LogResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging message: {ex.Message}");
            return new LogResponse { Success = false };
        }
    }
}