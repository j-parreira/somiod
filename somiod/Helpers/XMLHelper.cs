﻿/**
* @brief Somiod - Projeto de Integração de Sistemas
* @date 2024-12-18
* @authors Diogo Abegão, João Parreira, Marcelo Oliveira, Pedro Barbeiro
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Text;

namespace somiod.Helpers
{
    public class XMLHelper
    {
        // Method to validate XML against XSD
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

        // Method to deserialize XML
        public static T DeserializeXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        // Method to serialize XML with UTF-8 encoding
        public static string SerializeXmlUtf8<T>(T model)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (Utf8StringWriter stringWriter = new Utf8StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    Indent = false,
                    OmitXmlDeclaration = false
                };
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, model);
                }

                return stringWriter.ToString();
            }
        }

        // Class to write XML with UTF-8 encoding
        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}