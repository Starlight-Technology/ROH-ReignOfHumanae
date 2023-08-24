// Ignore Spelling: Utils

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ROH.Utils.ApiConfiguration
{
    public class ApiConfigReader
    {
        private readonly XDocument _config;

        public enum ApiUrl
        {
            Version
        }

        public ApiConfigReader()
        {
            _config = XDocument.Load("~/api-config.xml");
        }

        public Dictionary<ApiUrl, Uri> GetApiUrl()
        {
            var apiUrls = new Dictionary<ApiUrl, Uri>();

            foreach (var serviceElement in _config.Descendants("Service"))
            {
                var serviceName = Enum.Parse<ApiUrl>(serviceElement.Attribute("name").Value);
                var serviceUrl = new Uri(serviceElement.Attribute("url").Value);

                apiUrls.Add(serviceName, serviceUrl);
            }

            return apiUrls;
        }
    }
}
