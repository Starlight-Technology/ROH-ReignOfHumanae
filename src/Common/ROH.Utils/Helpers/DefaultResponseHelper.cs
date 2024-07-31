using Newtonsoft.Json;

using ROH.StandardModels.Response;

using System;

namespace ROH.Utils.Helpers
{
    public static class DefaultResponseHelper
    {
        public static T ResponseToModel<T>(this DefaultResponse response)
        {
            try
            {
                return response != null && response.ObjectResponse != null
                    ? JsonConvert.DeserializeObject<T>(response.ObjectResponse.ToString()) ?? throw new InvalidCastException()
                    : throw new InvalidCastException();
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Can't convert object response to {typeof(T)}.");
            }
        }

        public static DefaultResponse MapObjectResponse<T>(this DefaultResponse response)
        {
            try
            {
                if (response.ObjectResponse != null)
                {
                    string objectJson = JsonConvert.SerializeObject(response.ObjectResponse);
                    T model = JsonConvert.DeserializeObject<T>(objectJson);
                    return new DefaultResponse(model, response.HttpStatus, response.Message);
                }

                return new DefaultResponse(null, response.HttpStatus, response.Message);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Can't convert object response to {typeof(T)}.");
            }
        }
    }
}