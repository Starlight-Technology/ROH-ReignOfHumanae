using System.Net;

namespace ROH.Models.Response
{
    public record DefaultResponse(object? ObjectResponse = null,
                                  HttpStatusCode HttpStatus = HttpStatusCode.OK,
                                  string Message = "");
}
