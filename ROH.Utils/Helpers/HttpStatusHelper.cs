using System.Net;

namespace ROH.Utils.Helpers
{
    public static class HttpStatusHelper
    {
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode)
        {
            int numericStatusCode = (int)statusCode;
            return numericStatusCode >= 200 && numericStatusCode <= 299;
        }
        public static bool IsInfoStatusCode(this HttpStatusCode statusCode)
        {
            int numericStatusCode = (int)statusCode;
            return numericStatusCode >= 100 && numericStatusCode <= 199;
        }
        public static bool IsRedirectStatusCode(this HttpStatusCode statusCode)
        {
            int numericStatusCode = (int)statusCode;
            return numericStatusCode >= 300 && numericStatusCode <= 399;
        }
        public static bool IsClientErrorStatusCode(this HttpStatusCode statusCode)
        {
            int numericStatusCode = (int)statusCode;
            return numericStatusCode >= 400 && numericStatusCode <= 499;
        }
        public static bool IsServerErrorStatusCode(this HttpStatusCode statusCode)
        {
            int numericStatusCode = (int)statusCode;
            return numericStatusCode >= 500 && numericStatusCode <= 599;
        }
    }
}
