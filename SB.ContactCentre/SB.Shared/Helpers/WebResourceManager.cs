using System;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Helpers
{
    public class WebResourceManager
    {
        private readonly IOrganizationService _service;
        private readonly Entity _resource;
        private readonly string _webResourceContent;

        public WebResourceManager(string resourceName, IOrganizationService service)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new Exception($"{nameof(resourceName)} is null or white space");
            }

            _service = service;

            _resource = GetResource(resourceName);

            _webResourceContent = ConvertContentToXmlString();
        }

        public string GetStringValue(string name)
        {
            var value = string.Empty;

            var xml = new XmlDocument();
            xml.LoadXml(_webResourceContent.Substring(_webResourceContent.IndexOf(Environment.NewLine, StringComparison.Ordinal)));

            var nodeList = xml.SelectNodes($"/root/data[@name='{name}']");

            if (nodeList == null || nodeList.Count <= 0) return value;

            value = nodeList[0].InnerText.Replace("  ", "").Replace("\r", "").Replace("\n", "");

            return value;
        }

        private string ConvertContentToXmlString()
        {
            var webResourceContent = string.Empty;

            if (!_resource.Attributes.Contains("content")) return webResourceContent;

            var binary = Convert.FromBase64String(_resource.Attributes["content"].ToString());

            webResourceContent = Encoding.UTF8.GetString(binary);

            return webResourceContent;
        }

        private Entity GetResource(string resourceName)
        {
            var query = new QueryExpression
            {
                EntityName = "webresource",
                ColumnSet = new ColumnSet("name", "content"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "name",
                            Operator = ConditionOperator.Equal,
                            Values = { resourceName }
                        }
                    }
                }
            };

            var resource =  _service.RetrieveMultiple(query).Entities.FirstOrDefault();

            if (resource == null)
            {
                throw new Exception($"Resource {resourceName} not found");
            }

            return resource;
        }
    }
}