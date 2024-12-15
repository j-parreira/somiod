using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Application = somiod.Models.Application;

namespace CellPhoneApp
{
    public partial class Form1 : Form
    {
        // baseURI is the base URI of the somiod server.
        string baseURI = "http://localhost:51967/api/somiod/";
        public Form1()
        {
            InitializeComponent();
        }

        // Form1_Load method is called when the form is loaded and waits for 5 seconds before calling LoadAppsIntoComboBox method.
        private async void Form1_Load(object sender, EventArgs e)
        {
            Thread.Sleep(5000);
            await CreateApplicationAsync("mobile_application");
            LoadAppsIntoComboBox();
        }

        // CreateApplicationAsync method creates an application with the given name
        private async Task CreateApplicationAsync(string applicationName)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Application app = new Application
                    {
                        id = 0,
                        name = applicationName,
                        creation_datetime = DateTime.Now
                    };
                    string fullURI = baseURI + applicationName;
                    HttpResponseMessage response_1 = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response_1.StatusCode;
                    if (statusCode == 404)
                    {
                        fullURI = baseURI;
                        string requestBody = XMLHelper.SerializeXmlUtf8<Application>(app).ToString().Trim();
                        HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                        HttpResponseMessage response_2 = await client.PostAsync(fullURI, httpContent);
                        int statusCode_2 = (int)response_2.StatusCode;
                        if (statusCode_2 != 201)
                        {
                            throw new Exception("Error creating container");
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // LoadAppsIntoComboBox method loads the applications into the comboBoxApplication.
        private void LoadAppsIntoComboBox()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    comboBoxApplication.Items.Clear();
                    string fullURI = baseURI;
                    string header = "somiod-locate";
                    string headerValue = "application";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseBody);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ArrayOfString/string");
                    comboBoxApplication.Items.Add("Select an application");
                    foreach (XmlNode node in xmlNodeList)
                    {
                        comboBoxApplication.Items.Add(node.InnerText);
                    }
                    comboBoxApplication.SelectedIndex = 0;
                    comboBoxApplication.SelectedIndexChanged += ComboBoxApplication_SelectedIndexChanged;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // LoadContainerIntoComboBox method loads the containers into the comboBoxContainer.
        private void LoadContainerIntoComboBox()
        {
            if (comboBoxApplication.SelectedIndex == 0)
            {
                comboBoxContainer.Items.Clear();
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    comboBoxContainer.Items.Clear();
                    string applicationName = comboBoxApplication.SelectedItem.ToString();
                    string fullURI = baseURI + "/" + applicationName;
                    string header = "somiod-locate";
                    string headerValue = "container";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseBody);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ArrayOfString/string");
                    comboBoxContainer.Items.Add("Select a container");
                    foreach (XmlNode node in xmlNodeList)
                    {
                        comboBoxContainer.Items.Add(node.InnerText);
                    }
                    comboBoxContainer.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // ComboBoxApplication_SelectedIndexChanged method is called when the comboBoxApplication is changed and calls LoadContainerIntoComboBox method.
        private void ComboBoxApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadContainerIntoComboBox();
        }

        // buttonOn_Click method is called when the buttonOn is clicked and sends a POST request to the server to create a record with content "ON".
        private void buttonOn_Click(object sender, EventArgs e)
        {
            if (comboBoxApplication.SelectedIndex == 0)
            {
                return;
            }
            if (comboBoxContainer.SelectedIndex == 0)
            {
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Record record = new Record
                    {
                        id = 0,
                        name = "rec",
                        creation_datetime = DateTime.Now,
                        parent = 0,
                        content = "ON"
                    };
                    string applicationName = comboBoxApplication.SelectedItem.ToString();
                    string containerName = comboBoxContainer.SelectedItem.ToString();
                    string fullURI = baseURI + "/" + applicationName + "/" + containerName;
                    string header = "res_type";
                    string headerValue = "record";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    string requestBody = XMLHelper.SerializeXmlUtf8<Record>(record).ToString().Trim();
                    HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PostAsync(fullURI, httpContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(responseBody);
                        XmlNode xmlNode = xmlDocument.SelectSingleNode("/Record/name");
                        string name = xmlNode.InnerText;
                        textBoxResult.Text = $"Record {name} created.";
                    }
                    else
                    {
                        textBoxResult.Text = "Failure! Record not sent.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonOff_Click method is called when the buttonOff is clicked and sends a POST request to the server to create a record with content "OFF".
        private void buttonOff_Click(object sender, EventArgs e)
        {
            if (comboBoxApplication.SelectedIndex == 0)
            {
                return;
            }
            if (comboBoxContainer.SelectedIndex == 0)
            {
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Record record = new Record
                    {
                        id = 0,
                        name = "rec",
                        creation_datetime = DateTime.Now,
                        parent = 0,
                        content = "OFF"
                    };
                    string applicationName = comboBoxApplication.SelectedItem.ToString();
                    string containerName = comboBoxContainer.SelectedItem.ToString();
                    string fullURI = baseURI + "/" + applicationName + "/" + containerName;
                    string header = "res_type";
                    string headerValue = "record";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    string requestBody = XMLHelper.SerializeXmlUtf8<Record>(record).ToString().Trim();
                    HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PostAsync(fullURI, httpContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(responseBody);
                        XmlNode xmlNode = xmlDocument.SelectSingleNode("/Record/name");
                        string name = xmlNode.InnerText;
                        textBoxResult.Text = $"Record {name} created.";
                    }
                    else
                    {
                        textBoxResult.Text = "Failure! Record not sent.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
