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

namespace CellPhoneApp
{
    public partial class Form1 : Form
    {
        string baseURI = "http://localhost:51897/api/somiod/";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread.Sleep(5000);
            LoadAppsIntoComboBox();
        }

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

        private void ComboBoxApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadContainerIntoComboBox();
        }

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
                        Id = 0,
                        Name = "rec",
                        CreationDateTime = DateTime.Now,
                        Parent = 0,
                        Content = "ON"
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
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseBody);
                    XmlNode xmlNode = xmlDocument.SelectSingleNode("/Record/Name");
                    string name = xmlNode.InnerText;
                    if (response.IsSuccessStatusCode)
                    {
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
                        Id = 0,
                        Name = "rec",
                        CreationDateTime = DateTime.Now,
                        Parent = 0,
                        Content = "OFF"
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
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseBody);
                    XmlNode xmlNode = xmlDocument.SelectSingleNode("/Record/Name");
                    string name = xmlNode.InnerText;
                    if (response.IsSuccessStatusCode)
                    {
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
