// Ignore Spelling: Api Utils

using Newtonsoft.Json;

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

            UploadVersionFile,
            GetAllVersionFiles
        }

        private static readonly Dictionary<Services, Uri> _servicesUrl = new Dictionary<Services, Uri>
        {
            #region VERSION

            {Services.GetCurrentVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetCurrentVersion" ) },
            {Services.CreateNewVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"CreateNewVersion" ) },
            {Services.GetAllVersionsPaginated, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetAllVersionsPaginated" ) },
            {Services.GetAllReleasedVersionsPaginated, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetAllReleasedVersionsPaginated" ) },
            {Services.GetVersionDetails, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetVersionDetails" ) },
            #endregion VERSION

            #region FILES

             {Services.UploadVersionFile, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.VersionFile),"UploadFile" ) },
             {Services.GetAllVersionFiles, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.VersionFile),"GetAllVersionFiles" ) }
            #endregion FILES
        };

        public async Task<string> Get<T>(Services service, T parametersObject)
        {
            using HttpClient client = new HttpClient();

            string param = GetParams(parametersObject);

            HttpResponseMessage response = await client.GetAsync(_servicesUrl.GetValueOrDefault(service) + param);

            return await response.Content.ReadAsStringAsync();
        }

        private static string GetParams<T>(T parametersObject)
        {
            StringBuilder parameters = new StringBuilder();
            string param = "";

            if (parametersObject != null)
            {
                var properties = typeof(T).GetProperties();

                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(parametersObject);
                    var encodedValue = Uri.EscapeDataString(value?.ToString() ?? "");

                    _ = i == 0
                        ? parameters.Append($"?{properties[i].Name}={encodedValue}")
                        : parameters.Append($"&{properties[i].Name}={encodedValue}");
                }

                param = parameters.ToString();
            }

            return param;
        }

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

            string param = GetParams(parametersObject);

            HttpResponseMessage response = await client.DeleteAsync(_servicesUrl.GetValueOrDefault(service) + param);

            return await response.Content.ReadAsStringAsync();
        }
    }
}