// Ignore Spelling: Utils

using Newtonsoft.Json;

using ROH.StandardModels.Response;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Utils.ApiConfiguration
{
    public class Gateway
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
            UploadFile,
            GetAllVersionFiles
        }

        private static readonly Dictionary<Services, Uri> _gatewayServiceUrl = new Dictionary<Services, Uri>
        {
            #region VERSION

            {Services.GetCurrentVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay),"Api/Version/GetCurrentVersion" ) },
            {Services.CreateNewVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay),"Api/Version/CreateNewVersion" ) },
            {Services.GetAllVersionsPaginated, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay),"Api/Version/GetAllVersionsPaginated" ) },
            {Services.GetAllReleasedVersionsPaginated, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay),"Api/Version/GetAllReleasedVersionsPaginated" ) },
            {Services.GetVersionDetails, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay),"Api/Version/GetVersionDetails" ) },
            #endregion VERSION

            #region FILES

             {Services.UploadFile, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay),"Api/VersionFile/UploadFile" ) },
             {Services.GetAllVersionFiles, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.GateWay),"Api/VersionFile/GetAllVersionFiles" ) }
            #endregion FILES
        };

        public async Task<DefaultResponse?> Get<T>(Services service, T parametersObject)
        {
            using HttpClient client = new HttpClient();

            string param = GetParams(parametersObject);

            HttpResponseMessage response = await client.GetAsync(_gatewayServiceUrl.GetValueOrDefault(service) + param);

            if (response != null)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
            }

            return new DefaultResponse(message: "Error, the connection has failed!");
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

        public async Task<DefaultResponse?> Post(Services service, object objectToSend)
        {
            using HttpClient client = new HttpClient();

            string jsonContent = JsonConvert.SerializeObject(objectToSend);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(_gatewayServiceUrl.GetValueOrDefault(service), httpContent);

            if (response != null)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
            }

            return new DefaultResponse(message: "Error, the connection has failed!");
        }

        public async Task<DefaultResponse?> Update(Services service, object objectToSend)
        {
            using HttpClient client = new HttpClient();

            string jsonContent = JsonConvert.SerializeObject(objectToSend);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(_gatewayServiceUrl.GetValueOrDefault(service), httpContent);

            if (response != null)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
            }

            return new DefaultResponse(message: "Error, the connection has failed!");
        }

        public async Task<DefaultResponse?> Delete<T>(Services service, T parametersObject)
        {
            using HttpClient client = new HttpClient();

            string param = GetParams(parametersObject);        

            HttpResponseMessage response = await client.DeleteAsync(_gatewayServiceUrl.GetValueOrDefault(service) + param);

            if (response != null)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
            }

            return new DefaultResponse(message: "Error, the connection has failed!");
        }
    }
}