using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Xml;

namespace somiod.Handlers
{
    public class XMLValidator
    {
        public static bool ValidateWithXSD<T>(T model, string xsdPath, out string validationError)
        {
            validationError = string.Empty;
            string localValidationError = string.Empty;

            try
            {
                // Serialize the model to an XML string
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                string xmlData;
                using (StringWriter stringWriter = new StringWriter())
                {
                    serializer.Serialize(stringWriter, model);
                    xmlData = stringWriter.ToString();
                }

                // Set up the XSD validation
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, xsdPath);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    localValidationError = e.Message;
                };

                // Validate the XML
                using (StringReader stringReader = new StringReader(xmlData))
                using (XmlReader reader = XmlReader.Create(stringReader, settings))
                {
                    while (reader.Read()) { }
                }

                // If no validation error was set, validation succeeded
                validationError = localValidationError;
                return string.IsNullOrEmpty(validationError);
            }
            catch (Exception ex)
            {
                validationError = $"Exception during validation: {ex.Message}";
                return false;
            }
        }
    }
}