using System.Net;

namespace ROH.StandardModels.Response
{
    public class DefaultResponse
    {
        public object? ObjectResponse { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public string Message { get; set; }

        public DefaultResponse(object? objectResponse = null, HttpStatusCode httpStatus = HttpStatusCode.OK, string message = "")
        {
            ObjectResponse = objectResponse;
            HttpStatus = httpStatus;
            Message = message;
        }
    }
}