﻿// Ignore Spelling: Api Utils

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Utils.ApiConfiguration
{
    public class Api
    {
        private static readonly ApiConfigReader _apiConfig = new ApiConfigReader();

        private static readonly Dictionary<ApiUrl, Uri> _apiUrl = _apiConfig.GetApiUrl();

        public enum Services
        {
            GetCurrentVersion,
            CreateNewVersion,
            GetAllVersionsPaginated,
            GetAllReleasedVersionsPaginated,
            GetVersionDetails,
            ReleaseVersion,

            UploadVersionFile,
            GetAllVersionFiles,
            DownloadFile,

            CreateNewUser,
            FindUserByEmail,
            FindUserByUserName,
            GetUserByGuid,
            GetAccountByUserGuid,
            UpdateAccount,

            Login
        }

        private static readonly Dictionary<Services, Uri> _servicesUrl = new Dictionary<Services, Uri>
        {
            #region VERSION

            {Services.GetCurrentVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetCurrentVersion" ) },
            {Services.CreateNewVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"CreateNewVersion" ) },
            {Services.GetAllVersionsPaginated, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetAllVersionsPaginated" ) },
            {Services.GetAllReleasedVersionsPaginated, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetAllReleasedVersionsPaginated" ) },
            {Services.GetVersionDetails, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetVersionDetails" ) },
            {Services.ReleaseVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"ReleaseVersion" ) },

            #endregion VERSION

            #region FILES

             {Services.UploadVersionFile, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.VersionFile),"UploadFile" ) },
             {Services.GetAllVersionFiles, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.VersionFile),"GetAllVersionFiles" ) },
             {Services.DownloadFile, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.VersionFile),"DownloadFile" ) },

            #endregion FILES

            #region ACCOUNT

             {Services.CreateNewUser, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Account),"CreateNewUser" ) },
             {Services.FindUserByEmail, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Account),"FindUserByEmail" ) },
             {Services.FindUserByUserName, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Account),"FindUserByUserName" ) },
             {Services.GetUserByGuid, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Account),"GetUserByGuid" ) },
             {Services.GetAccountByUserGuid, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Account),"GetAccountByUserGuid" ) },
             {Services.UpdateAccount, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Account),"UpdateAccount" ) },

            #endregion ACCOUNT

            #region LOGIN

            { Services.Login, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Login), "Login")}

            #endregion LOGIN
        };

        public async Task<string> Get<T>(Services service, T parametersObject)

        {
            using HttpClient client = new HttpClient();
            string param = string.Empty;

            if (!Equals(parametersObject, default(T)))
            {
                param = GetParams(parametersObject!);
            }

            HttpResponseMessage response = await client.GetAsync(_servicesUrl.GetValueOrDefault(service) + param);

            return await response.Content.ReadAsStringAsync();
        }

        public string GetParams(object parametersObject)
        {
            if (parametersObject == null)
            {
                return string.Empty;
            }

            string json = JsonConvert.SerializeObject(parametersObject);
            JObject jObject = JObject.Parse(json);

            StringBuilder parameters = new StringBuilder();

            foreach (JProperty property in jObject.Properties())
            {
                JToken value = property.Value;
                if (value != null && IsSimpleType(value.Type))
                {
                    string encodedValue = Uri.EscapeDataString(value.ToString());
                    _ = parameters.Append(parameters.Length == 0 ? "?" : "&");
                    _ = parameters.Append($"{property.Name}={encodedValue}");
                }
                else
                {
                    throw new InvalidOperationException("Can't convert object to query string.");
                }
            }

            return parameters.ToString();
        }

        private static bool IsSimpleType(JTokenType type) => type == JTokenType.String || type == JTokenType.Integer || type == JTokenType.Float ||
                   type == JTokenType.Boolean || type == JTokenType.Date || type == JTokenType.Guid;

        public async Task<string> Post(Services service, object objectToSend)
        {
            using HttpClient client = new HttpClient();

            string jsonContent = JsonConvert.SerializeObject(objectToSend);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(_servicesUrl.GetValueOrDefault(service), httpContent);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Update(Services service, object objectToSend)
        {
            using HttpClient client = new HttpClient();

            string jsonContent = JsonConvert.SerializeObject(objectToSend);

            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(_servicesUrl.GetValueOrDefault(service), httpContent);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Delete<T>(Services service, T parametersObject)
        {
            using HttpClient client = new HttpClient();

            string param = string.Empty;

            if (!Equals(parametersObject, default(T)))
            {
                param = GetParams(parametersObject!);
            }

            HttpResponseMessage response = await client.DeleteAsync(_servicesUrl.GetValueOrDefault(service) + param);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
