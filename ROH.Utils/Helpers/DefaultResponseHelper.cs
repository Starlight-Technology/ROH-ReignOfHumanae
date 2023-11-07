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
                return JsonConvert.DeserializeObject<T>(response.ObjectResponse?.ToString());
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Can't convert object response to {typeof(T)}.");
            }
        }
    }
}
