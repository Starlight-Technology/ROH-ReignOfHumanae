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
            CreateNewVersion
        }

        private static readonly Dictionary<Services, Uri> _servicesUrl = new Dictionary<Services, Uri>
        {
            #region VERSION

            {Services.GetCurrentVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"GetCurrentVersion" ) },
            {Services.CreateNewVersion, new Uri(_apiUrl.GetValueOrDefault(ApiUrl.Version),"CreateNewVersion" ) }
            #endregion VERSION
        };

        public async Task<string> Get(Services service, List<ApiParameters> apiParameters)
        {
            using HttpClient client = new HttpClient();

            StringBuilder parameters = new StringBuilder();
            string param = "";

            if (apiParameters.Count > 0)
            {
                for (int i = 0; i < apiParameters.Count; i++)
                {
                    if (i == 0)
                    {
                        parameters.Append($"?{apiParameters[i].Name}={apiParameters[i].Value}");
                    }
                    else
                    {
                        parameters.Append($"&{apiParameters[i].Name}={apiParameters[i].Value}");
                    }
                }

                param = parameters.ToString();
            }

            var response = await client.GetAsync(_servicesUrl.GetValueOrDefault(service) + param);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(Services service, object objectToSend)
        {
            using HttpClient client = new HttpClient();

            var jsonContent = JsonConvert.SerializeObject(objectToSend);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_servicesUrl.GetValueOrDefault(service), httpContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Update(Services service, object objectToSend)
        {
            using HttpClient client = new HttpClient();

            var jsonContent = JsonConvert.SerializeObject(objectToSend);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(_servicesUrl.GetValueOrDefault(service), httpContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Delete(Services service, List<ApiParameters> apiParameters)
        {
            using HttpClient client = new HttpClient();

            StringBuilder parameters = new StringBuilder();
            string param = "";

            if (apiParameters.Count > 0)
            {
                parameters.Append("?");

                for (int i = 0; i < apiParameters.Count; i++)
                {
                    if (i == 0)
                    {
                        parameters.Append($"{apiParameters[i].Name}={apiParameters[i].Value}");
                    }
                    else
                    {
                        parameters.Append($"&{apiParameters[i].Name}={apiParameters[i].Value}");
                    }
                }

                param = parameters.ToString();
            }

            var response = await client.DeleteAsync(_servicesUrl.GetValueOrDefault(service) + param);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}