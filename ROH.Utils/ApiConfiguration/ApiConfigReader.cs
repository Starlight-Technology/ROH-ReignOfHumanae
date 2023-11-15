﻿// Ignore Spelling: Utils

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace ROH.Utils.ApiConfiguration
{
    public class ApiConfigReader
    {
        private readonly XDocument _config;

        public enum ApiUrl
        {
            Version,
            VersionFile,
            GateWay,
        }

        public ApiConfigReader()
        {

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            string xmlFilePath = Path.Combine(assemblyDirectory, "api-config.xml");

            _config = XDocument.Load(xmlFilePath);
        }

        public Dictionary<ApiUrl, Uri> GetApiUrl()
        {
            // Select the appropriate configuration based on the build configuration
#if DEBUG
            // Debug configuration
            string configuration = "Develop";
#elif TEST
            string configuration = "Test";
#else
        // Release configuration
        string configuration = "Release";
#endif
            var selectedConfiguration = _config.Descendants(configuration).FirstOrDefault() ?? throw new InvalidOperationException($"Configuration '{configuration}' not found in api-config.xml");
            var apiUrls = new Dictionary<ApiUrl, Uri>();

            foreach (var serviceElement in selectedConfiguration.Descendants("Service"))
            {
                var serviceName = Enum.Parse<ApiUrl>(serviceElement.Attribute("name").Value);
                var serviceUrl = new Uri(serviceElement.Attribute("url").Value);

                apiUrls.Add(serviceName, serviceUrl);
            }

            return apiUrls;
        }
    }

}