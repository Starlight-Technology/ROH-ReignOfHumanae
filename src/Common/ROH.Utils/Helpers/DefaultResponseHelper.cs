//-----------------------------------------------------------------------
// <copyright file="DefaultResponseHelper.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Newtonsoft.Json;

using ROH.StandardModels.Response;

using System;

namespace ROH.Utils.Helpers
{
    public static class DefaultResponseHelper
    {
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

        public static T ResponseToModel<T>(this DefaultResponse response)
        {
            try
            {
                return ((response != null) && (response.ObjectResponse != null))
                    ? (JsonConvert.DeserializeObject<T>(response.ObjectResponse.ToString()) ??
                        throw new InvalidCastException())
                    : throw new InvalidCastException();
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Can't convert object response to {typeof(T)}.");
            }
        }

        public static object JsonToObject<T>(this string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new ArgumentException("Input JSON string cannot be null or whitespace.", nameof(json));
                }

                T result = JsonConvert.DeserializeObject<T>(json)
                           ?? throw new InvalidCastException($"Deserialization resulted in a null object for type {typeof(T)}.");

                return result;
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Can't convert Json to {typeof(T)}.");
            }
        }

        public static string ObjectToJson(this object obj)
        {
            try
            {
                return obj == null ? string.Empty : JsonConvert.SerializeObject(obj);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Can't convert object to Json.");
            }
        }
    }
}