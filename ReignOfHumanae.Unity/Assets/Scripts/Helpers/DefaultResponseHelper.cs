using Assets.Scripts.Models.Response;

using System;

using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class DefaultResponseHelper
    {
        public static T ResponseToModel<T>(this DefaultResponse response)
        {
            try
            {
                return response != null && response.ObjectResponse != null
                    ? JsonUtility.FromJson<T>(response.ObjectResponse.ToString()) ?? throw new InvalidCastException()
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
                    string objectJson = JsonUtility.ToJson(response.ObjectResponse);
                    T model = JsonUtility.FromJson<T>(objectJson);
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