//-----------------------------------------------------------------------
// <copyright file="Gateway.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
// Ignore Spelling: Utils

using Newtonsoft.Json;

using ROH.StandardModels.Response;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Utils.ApiConfiguration
{
    public class Gateway
    {
        private const string ERROR_MESSAGE = "Error, the connection has failed!";
        private const string UNAUTHORIZED_MESSAGE = "You must to be logged to do that.";
        private static readonly ApiConfigReader _apiConfig = new ApiConfigReader();
        private static readonly Dictionary<ApiUrl, Uri> _apiUrl = _apiConfig.GetApiUrl();

        private static readonly Dictionary<Services, Uri> _gatewayServiceUrl = new Dictionary<Services, Uri> {

            #region VERSION

            {
                Services.GetCurrentVersion,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Version/GetCurrentVersion")
            },
            {
                Services.CreateNewVersion,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Version/CreateNewVersion")
            },
            {
                Services.GetAllVersionsPaginated,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Version/GetAllVersionsPaginated")
            },
            {
                Services.GetAllReleasedVersionsPaginated,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Version/GetAllReleasedVersionsPaginated")
            },
            {
                Services.GetVersionDetails,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Version/GetVersionDetails")
            },
            {
                Services.ReleaseVersion,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Version/ReleaseVersion")
            },

            #endregion VERSION

            #region FILES

            { Services.UploadFile, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/VersionFile/UploadFile") },
            {
                Services.GetAllVersionFiles,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/VersionFile/GetAllVersionFiles")
            },
            {
                Services.DownloadFile,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/VersionFile/DownloadFile")
            },

            #endregion FILES

            #region ACCOUNT

            { Services.CreateNewUser, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Account/CreateNewUser") },
            {
                Services.FindUserByEmail,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Account/FindUserByEmail")
            },
            {
                Services.FindUserByUserName,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Account/FindUserByUserName")
            },
            { Services.GetUserByGuid, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Account/GetUserByGuid") },
            {
                Services.GetAccountByUserGuid,
                new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Account/GetAccountByUserGuid")
            },
            { Services.UpdateAccount, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Account/UpdateAccount") },
            #endregion ACCOUNT

            #region LOGIN

            { Services.Login, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay), "Api/Account/Login") },
            #endregion LOGIN

            #region PLAYER

            { Services.CreateCharacter, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Player), "Api/Player/CreateCharacter")},
            { Services.GetAccountCaracters, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Player), "Api/Player/GetAccountCaracters")},
            { Services.GetCharacter, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Player), "Api/Player/GetCharacter")},
            { Services.SavePosition, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.PlayerState), "")},
            #endregion PLAYER

        };

        private readonly Api _api = new Api();

        private readonly DefaultResponse? _errorResponse = new DefaultResponse(
            httpStatus: HttpStatusCode.BadRequest,
            message: ERROR_MESSAGE);

        private readonly DefaultResponse? _unauthorizedResponse = new DefaultResponse(
            httpStatus: HttpStatusCode.Unauthorized,
            message: UNAUTHORIZED_MESSAGE);

        public async Task<DefaultResponse?> DeleteAsync<T>(Services service, T parametersObject, string token = "", CancellationToken cancellationToken = default)
        {
            var handler = new HttpClientHandler();
#if DEBUG
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) => true;
#endif
            using HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string param = string.Empty;

            if (!object.Equals(parametersObject, default(T)))
            {
                param = _api.GetParams(parametersObject!);
            }

            HttpResponseMessage response = await client.DeleteAsync(
                $"{_gatewayServiceUrl.GetValueOrDefault(service)}{param}", cancellationToken).ConfigureAwait(true);

            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return _unauthorizedResponse;

                string responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
            }

            return _errorResponse;
        }

        public async Task<DefaultResponse?> GetAsync<T>(Services service, T parametersObject, string token = "", CancellationToken cancellationToken = default)
        {
            try
            {
                var handler = new HttpClientHandler();
#if DEBUG
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) => true;
#endif
                using HttpClient client = new HttpClient(handler);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string param = string.Empty;

                if (!Equals(parametersObject, default(T)))
                {
                    param = _api.GetParams(parametersObject!);
                }

                HttpResponseMessage response = await client.GetAsync(
                    $"{_gatewayServiceUrl.GetValueOrDefault(service)}{param}", cancellationToken).ConfigureAwait(true);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return _unauthorizedResponse;

                    string responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                    return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
                }

                return _errorResponse;
            }
            catch (Exception e)
            {
                return new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: e.Message);
            }
        }

        public async Task<DefaultResponse?> PostAsync(Services service, object objectToSend, string token = "", CancellationToken cancellationToken = default)
        {
            var handler = new HttpClientHandler();
#if DEBUG
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) => true;
#endif
            using HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonConvert.SerializeObject(objectToSend);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(
                _gatewayServiceUrl.GetValueOrDefault(service),
                httpContent, cancellationToken).ConfigureAwait(true);

            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return _unauthorizedResponse;

                string responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
            }

            return _errorResponse;
        }

        public async Task<DefaultResponse?> UpdateAsync(Services service, object objectToSend, string token = "", CancellationToken cancellationToken = default)
        {
            var handler = new HttpClientHandler();
#if DEBUG
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) => true;
#endif
            using HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonConvert.SerializeObject(objectToSend);

            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(
                _gatewayServiceUrl.GetValueOrDefault(service),
                httpContent, cancellationToken).ConfigureAwait(true);

            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return _unauthorizedResponse;

                string responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
            }

            return _errorResponse;
        }

        public enum Services
        {
            #region VERSION

            GetCurrentVersion,
            CreateNewVersion,
            GetAllVersionsPaginated,
            GetAllReleasedVersionsPaginated,
            GetVersionDetails,
            ReleaseVersion,

            #endregion VERSION

            #region VERSIONFILE

            UploadFile,
            GetAllVersionFiles,
            DownloadFile,

            #endregion VERSIONFILE

            #region ACCOUNT

            CreateNewUser,
            FindUserByEmail,
            FindUserByUserName,
            GetUserByGuid,
            GetAccountByUserGuid,
            UpdateAccount,

            #endregion ACCOUNT

            #region LOGIN

            Login,

            #endregion LOGIN

            #region LOG

            Log,

            #endregion LOG

            #region PLAYER
            CreateCharacter,
            GetAccountCaracters,
            GetCharacter,
            SavePosition,
            #endregion PLAYER
        }
    }
}