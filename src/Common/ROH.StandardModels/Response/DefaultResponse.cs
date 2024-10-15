//-----------------------------------------------------------------------
// <copyright file="DefaultResponse.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Net;

namespace ROH.StandardModels.Response
{
    public class DefaultResponse
    {
        public DefaultResponse(
            object? objectResponse = null,
            HttpStatusCode httpStatus = HttpStatusCode.OK,
            string message = "")
        {
            ObjectResponse = objectResponse;
            HttpStatus = httpStatus;
            Message = message;
        }

        public HttpStatusCode HttpStatus { get; set; }

        public string Message { get; set; }

        public object? ObjectResponse { get; set; }
    }
}