using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace somiod.Handlers
{
    public class XMLHandler
    {
        public static bool ValidateXml(string xmlContent, string xsdPath, out string validationError)
        {
            validationError = null;
            string localValidationError = string.Empty;
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, xsdPath);
                settings.ValidationType = ValidationType.Schema;

                settings.ValidationEventHandler += (sender, args) =>
                {
                    if (args.Severity == XmlSeverityType.Error || args.Severity == XmlSeverityType.Warning)
                    {
                        localValidationError = $"Exception during validation: {args.Message}";
                    }
                };

                using (StringReader stringReader = new StringReader(xmlContent))
                using (XmlReader reader = XmlReader.Create(stringReader, settings))
                {
                    while (reader.Read()) { }
                }

                validationError = localValidationError;
                return string.IsNullOrEmpty(validationError);
            }
            catch (Exception ex)
            {
                validationError = $"Exception during validation: {ex.Message}";
                return false;
            }
        }

        public static T DeserializeXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static string SerializeXml<T>(T model)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, model);
                return writer.ToString();
            }
        }
    }
}