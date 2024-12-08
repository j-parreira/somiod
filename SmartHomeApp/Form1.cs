using somiod.Helpers;
using somiod.Models;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = somiod.Models.Application;
using Container = somiod.Models.Container;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net;
using System.IO;
using System.Xml;

namespace SmartHomeApp
{
    public partial class Form1 : Form
    {
        string baseURI = "http://localhost:51897/api/somiod/";
        MqttClient mClient = new MqttClient(IPAddress.Parse("127.0.0.1"));
        string filename;
        int daysToKeep;
        string[] topics = { "smarthome/smarthome_all", "lighting/parking_light",
            "lighting/garden_light", "lighting/lighting_all",
            "heating/air_conditioning", "heating/heater",
            "heating/heating_all", "security/camera",
            "security/alarm", "security/security_all" };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mClient.Connect(Guid.NewGuid().ToString());
            LoadConfigValues();
            InitializeSmartHomeAsync();
            SubscribeToTopics(topics);
            PruneOldMessages(daysToKeep);
            labelParkingLightSwitch.Text = "ON";
            richTextBoxParkingLight.BackColor = Color.Yellow;
            labelGardenLightSwitch.Text = "ON";
            richTextBoxGardenLight.BackColor = Color.Yellow;
            labelAirConditioningSwitch.Text = "ON";
            richTextBoxAirConditioning.BackColor = Color.Blue;
            labelHeaterSwitch.Text = "ON";
            richTextBoxHeater.BackColor = Color.Red;
            labelCameraSwitch.Text = "ON";
            richTextBoxCamera.BackColor = Color.Green;
            labelAlarmSwitch.Text = "ON";
            richTextBoxAlarm.BackColor = Color.Red;
        }

        private async void InitializeSmartHomeAsync()
        {
            try
            {
                await CreateApplicationAsync("smarthome");
                await CreateApplicationAsync("lighting");
                await CreateApplicationAsync("heating");
                await CreateApplicationAsync("security");
                await CreateContainerAsync("smarthome", "smarthome_all");
                await CreateContainerAsync("lighting", "parking_light");
                await CreateContainerAsync("lighting", "garden_light");
                await CreateContainerAsync("lighting", "lighting_all");
                await CreateContainerAsync("heating", "air_conditioning");
                await CreateContainerAsync("heating", "heater");
                await CreateContainerAsync("heating", "heating_all");
                await CreateContainerAsync("security", "camera");
                await CreateContainerAsync("security", "alarm");
                await CreateContainerAsync("security", "security_all");
                await CreateNotification("smarthome", "smarthome_all");
                await CreateNotification("lighting", "parking_light");
                await CreateNotification("lighting", "garden_light");
                await CreateNotification("lighting", "lighting_all");
                await CreateNotification("heating", "air_conditioning");
                await CreateNotification("heating", "heater");
                await CreateNotification("heating", "heating_all");
                await CreateNotification("security", "camera");
                await CreateNotification("security", "alarm");
                await CreateNotification("security", "security_all");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization failed: {ex.Message}");
            }
        }

        private void LoadConfigValues()
        {
            try
            {
                filename = ConfigurationManager.AppSettings["filename"];
                if (string.IsNullOrWhiteSpace(filename))
                {
                    MessageBox.Show("Filename not set in configuration. Using default value.");
                    filename = "data.xml";
                }

                string daysToKeepValue = ConfigurationManager.AppSettings["daysToKeep"];
                if (int.TryParse(daysToKeepValue, out int parsedDays))
                {
                    daysToKeep = parsedDays;
                }
                else
                {
                    MessageBox.Show("Invalid daysToKeep value in configuration. Using default value.");
                    daysToKeep = 30;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading configuration: {ex.Message}");
            }
        }

        private async Task CreateApplicationAsync(string applicationName)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Application app = new Application
                    {
                        Id = 0,
                        Name = applicationName,
                        CreationDateTime = DateTime.Now
                    };
                    string fullURI = baseURI;
                    string requestBody = XMLHelper.SerializeXmlUtf8<Application>(app).ToString().Trim();
                    HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = await client.PostAsync(fullURI, httpContent);
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async Task CreateContainerAsync(string applicationName, string containerName)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Container cont = new Container
                    {
                        Id = 0,
                        Name = containerName,
                        CreationDateTime = DateTime.Now,
                        Parent = 0
                    };
                    string fullURI = baseURI + applicationName;
                    string requestBody = XMLHelper.SerializeXmlUtf8<Container>(cont).ToString().Trim();
                    HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = await client.PostAsync(fullURI, httpContent);
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async Task CreateNotification(string applicationName, string containerName)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Notification not = new Notification
                    {
                        Id = 0,
                        Name = "not",
                        CreationDateTime = DateTime.Now,
                        Parent = 0,
                        Event = "1",
                        Endpoint = "127.0.0.1",
                        Enabled = true
                    };
                    string fullURI = baseURI + applicationName + "/" + containerName;
                    string header = "res_type";
                    string headerValue = "notification";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    string requestBody = XMLHelper.SerializeXmlUtf8<Notification>(not);
                    HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = await client.PostAsync(fullURI, httpContent);
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void MClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            string body = Encoding.UTF8.GetString(e.Message);
            string[] vars = body.Split(';');
            string mqttEvent = vars[0];
            string message = vars[1];
            AppendMqttMessage(topic, mqttEvent, message);
            UpdateUI(topic, message);
        }

        private void SubscribeToTopics(string[] topics)
        {
            try
            {
                if (!mClient.IsConnected)
                {
                    throw new Exception("Error connecting to message broker...");
                }
                mClient.MqttMsgPublishReceived += MClient_MqttMsgPublishReceived;
                byte[] qosLevels = topics.Select(t => MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE).ToArray();
                mClient.Subscribe( topics, qosLevels);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateUI(string topic, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUI(topic, message)));
                return;
            }
            switch (topic)
            {
                case "smarthome/smarthome_all":
                    if (message == "ON")
                    {
                        labelParkingLightSwitch.Text = "ON";
                        richTextBoxParkingLight.BackColor = Color.Yellow;
                        labelGardenLightSwitch.Text = "ON";
                        richTextBoxGardenLight.BackColor = Color.Yellow;
                        labelAirConditioningSwitch.Text = "ON";
                        richTextBoxAirConditioning.BackColor = Color.Blue;
                        labelHeaterSwitch.Text = "ON";
                        richTextBoxHeater.BackColor = Color.Red;
                        labelCameraSwitch.Text = "ON";
                        richTextBoxCamera.BackColor = Color.Green;
                        labelAlarmSwitch.Text = "ON";
                        richTextBoxAlarm.BackColor = Color.Red;
                    }
                    else
                    {
                        labelParkingLightSwitch.Text = "OFF";
                        richTextBoxParkingLight.BackColor = Color.Black;
                        labelGardenLightSwitch.Text = "OFF";
                        richTextBoxGardenLight.BackColor = Color.Black;
                        labelHeaterSwitch.Text = "OFF";
                        richTextBoxHeater.BackColor = Color.Black;
                        labelAirConditioningSwitch.Text = "OFF";
                        richTextBoxAirConditioning.BackColor = Color.Black;
                        labelCameraSwitch.Text = "OFF";
                        richTextBoxCamera.BackColor = Color.Black;
                        labelAlarmSwitch.Text = "OFF";
                        richTextBoxAlarm.BackColor = Color.Black;
                    }
                    break;
                case "lighting/parking_light":
                    if (message == "ON")
                    {
                        labelParkingLightSwitch.Text = "ON";
                        richTextBoxParkingLight.BackColor = Color.Yellow;
                    }
                    else
                    {
                        labelParkingLightSwitch.Text = "OFF";
                        richTextBoxParkingLight.BackColor = Color.Black;
                    }
                    break;
                case "lighting/garden_light":
                    if (message == "ON")
                    {
                        labelGardenLightSwitch.Text = "ON";
                        richTextBoxGardenLight.BackColor = Color.Yellow;
                    }
                    else
                    {
                        labelGardenLightSwitch.Text = "OFF";
                        richTextBoxGardenLight.BackColor = Color.Black;
                    }
                    break;
                case "lighting/lighting_all":
                    if (message == "ON")
                    {
                        labelParkingLightSwitch.Text = "ON";
                        richTextBoxParkingLight.BackColor = Color.Yellow;
                        labelGardenLightSwitch.Text = "ON";
                        richTextBoxGardenLight.BackColor = Color.Yellow;
                    }
                    else
                    {
                        labelParkingLightSwitch.Text = "OFF";
                        richTextBoxParkingLight.BackColor = Color.Black;
                        labelGardenLightSwitch.Text = "OFF";
                        richTextBoxGardenLight.BackColor = Color.Black;
                    }
                    break;
                case "heating/air_conditioning":
                    if (message == "ON")
                    {
                        labelAirConditioningSwitch.Text = "ON";
                        richTextBoxAirConditioning.BackColor = Color.Blue;
                    }
                    else
                    {
                        // Turn off air conditioning
                        labelAirConditioningSwitch.Text = "OFF";
                        richTextBoxAirConditioning.BackColor = Color.Black;
                    }
                    break;
                case "heating/heater":
                    if (message == "ON")
                    {
                        // Turn on heater
                        labelHeaterSwitch.Text = "ON";
                        richTextBoxHeater.BackColor = Color.Red;
                    }
                    else
                    {
                        // Turn off heater
                        labelHeaterSwitch.Text = "OFF";
                        richTextBoxHeater.BackColor = Color.Black;
                    }
                    break;
                case "heating/heating_all":
                    if (message == "ON")
                    {
                        // Turn on all heating devices
                        labelAirConditioningSwitch.Text = "ON";
                        richTextBoxAirConditioning.BackColor = Color.Blue;
                        labelHeaterSwitch.Text = "ON";
                        richTextBoxHeater.BackColor = Color.Red;
                    }
                    else
                    {
                        // Turn off all heating devices
                        labelHeaterSwitch.Text = "OFF";
                        richTextBoxHeater.BackColor = Color.Black;
                        labelAirConditioningSwitch.Text = "OFF";
                        richTextBoxAirConditioning.BackColor = Color.Black;
                    }
                    break;
                case "security/camera":
                    if (message == "ON")
                    {
                        // Turn on camera
                        labelCameraSwitch.Text = "ON";
                        richTextBoxCamera.BackColor = Color.Green;
                    }
                    else
                    {
                        // Turn off camera
                        labelCameraSwitch.Text = "OFF";
                        richTextBoxCamera.BackColor = Color.Black;
                    }
                    break;
                case "security/alarm":
                    if (message == "ON")
                    {
                        // Turn on alarm
                        labelAlarmSwitch.Text = "ON";
                        richTextBoxAlarm.BackColor = Color.Red;
                    }
                    else
                    {
                        // Turn off alarm
                        labelAlarmSwitch.Text = "OFF";
                        richTextBoxAlarm.BackColor = Color.Black;
                    }
                    break;
                case "security/security_all":
                    if (message == "ON")
                    {
                        // Turn on all security devices
                        labelCameraSwitch.Text = "ON";
                        richTextBoxCamera.BackColor = Color.Green;
                        labelAlarmSwitch.Text = "ON";
                        richTextBoxAlarm.BackColor = Color.Red;
                    }
                    else
                    {
                        // Turn off all security devices
                        labelCameraSwitch.Text = "OFF";
                        richTextBoxCamera.BackColor = Color.Black;
                        labelAlarmSwitch.Text = "OFF";
                        richTextBoxAlarm.BackColor = Color.Black;
                    }
                    break;
                default:
                    break;
            }
        }

        private void AppendMqttMessage(string topic, string mqttEvent, string message)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);

            try
            {
                XDocument doc;

                // Load existing file or create a new one
                if (File.Exists(filePath))
                {
                    doc = XDocument.Load(filePath);
                }
                else
                {
                    doc = new XDocument(new XElement("MqttMessages"));
                }

                // Parse the incoming XML string into an XElement
                XElement messageElement;
                try
                {
                    //messageElement = XElement.Parse(xmlMessage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Invalid XML: {ex.Message}");
                    return;
                }

                // Add the parsed XElement to the root of the document
                //doc.Root.Add(messageElement);

                // Save the updated document
                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving MQTT message: {ex.Message}");
            }
        }


        private void PruneOldMessages(int daysToKeep)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.xml");

            try
            {
                if (!File.Exists(filePath))
                {
                    return;
                }
                XDocument doc = XDocument.Load(filePath);
                DateTime cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                doc.Root.Elements("MqttMessage").Where(m => DateTime.Parse(m.Element("Record").Element("CreationDateTime").Value) < cutoffDate).Remove();
                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error pruning old messages: {ex.Message}");
            }
        }
    }
}
