// Ignore Spelling: Api Utils

using Newtonsoft.Json;

using ROH.StandardModels.Response;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Utils
{
    public class Api
    {
        public enum Services
        {
            Version
        }

        private static readonly Dictionary<Services, string> _servicesUrl = new Dictionary<Services, string>
        {
            {Services.Version, "" }
        };

        public async Task<DefaultResponse?> Get(Services service, List<ApiParameters> apiParameters)
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

            var response = await client.GetAsync(_servicesUrl.GetValueOrDefault(service) + param);

            if (response != null)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<DefaultResponse>(responseJson);
            }

            return new DefaultResponse(message: "Error, the connection has failed!");

        }
    }

    public class ApiParameters
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ApiParameters()
        {
            Name = "";
            Value = "";
        }
    }
}
