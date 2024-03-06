// Ignore Spelling: Api Utils

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

            UploadVersionFile,
            GetAllVersionFiles,
            DownloadFile
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
             {Services.GetAllVersionFiles, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.VersionFile),"GetAllVersionFiles" ) },
             {Services.DownloadFile, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.VersionFile),"DownloadFile" ) }
            #endregion FILES
        };
        /*
        public async Task<string> Get<T>(Services service, T obj)
        {
            using HttpClient client = new HttpClient();

            StringBuilder parameters = new StringBuilder();
            string param = "";

            PropertyInfo[] properties = typeof(T).GetProperties();

            if (properties.Length > 0)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    string name = properties[i].Name;
                    object value = properties[i].GetValue(obj);

                     i == 0
                        ? parameters.Append($"?{name}={value}")
                        : parameters.Append($"&{name}={value}");
                }

                param = parameters.ToString();
            }

            HttpResponseMessage response = await client.GetAsync(_servicesUrl.GetValueOrDefault(service) + param);

            return await response.Content.ReadAsStringAsync();
        }
        */
        public async Task<string> Get<T>(Services service, T parametersObject)

        {
            using HttpClient client = new HttpClient();
            string param = string.Empty;

            if (parametersObject != null)
            {
                param = GetParams(parametersObject);
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

            if (parametersObject != null)
            {
                param = GetParams(parametersObject);
            }

            HttpResponseMessage response = await client.DeleteAsync(_servicesUrl.GetValueOrDefault(service) + param);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
