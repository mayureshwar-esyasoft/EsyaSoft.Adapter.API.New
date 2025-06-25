using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EsyaSoft.Adapter.API.Utils
{
    public class XMLHelper
    {
        
        public T Deserialize<T>(string input) where T : class
        {
            input = RemoveAllNamespaces(input.ToString());
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(input);
            string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
            return JsonConvert.DeserializeObject<T>(jsonText);
        }

        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }
        
        public string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespacesFromElement(XElement.Parse(xmlDocument));
            return xmlDocumentWithoutNs.ToString();
        }


        #region Private Methods
        //Core recursion function
        private XElement RemoveAllNamespacesFromElement(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespacesFromElement(el)));
        }

        private static void RemoveAllNamespacesChatGPT(XmlNode node)
        {
            if (node.Attributes != null && node.Attributes["xmlns"] != null)
            {
                node.Attributes.Remove(node.Attributes["xmlns"]);
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                RemoveAllNamespacesChatGPT(childNode);
            }
        }
        #endregion

    }

    public class IgnoreNamespaceXmlTextReader : XmlTextReader
    {
        public IgnoreNamespaceXmlTextReader(TextReader reader) : base(reader)
        {
        }
        public override string NamespaceURI => "";
    }
}
