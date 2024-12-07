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
        string[] topics2 = { "lighting/parking_light", "lighting/garden_light" };

        public Form1()
        {
            InitializeComponent();
            LoadConfigValues();
            InitializeSmartHomeAsync();
            SubscribeToNotifications(topics);
            PruneOldMessages(daysToKeep);
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
                    string requestBody = XMLHelper.SerializeXml<Application>(app).Trim();
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
                    string requestBody = XMLHelper.SerializeXml<Container>(cont).Trim();
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
                    string requestBody = XMLHelper.SerializeXml<Notification>(not).Trim();
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

        private void SubscribeToNotifications(string[] topics)
        {
            try
            {
                mClient.Connect(Guid.NewGuid().ToString());
                if (!mClient.IsConnected)
                {
                    throw new Exception("Error connecting to message broker...");
                }
                mClient.MqttMsgPublishReceived += MClient_MqttMsgPublishReceived;
                //byte[] qosLevels = topics.Select(t => MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE).ToArray();
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
                Console.WriteLine(qosLevels);
                mClient.Subscribe( topics2, qosLevels);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            Console.WriteLine($"Received message on topic: {topic}");
            string message = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine($"Message: {message}");
            MqttMessage mqttMessage = XMLHelper.DeserializeXml<MqttMessage>(message);
            AppendMqttMessage(mqttMessage);
            HandleEvent(topic, mqttMessage);
        }

        private void HandleEvent(string topic, MqttMessage mqttMessage)
        {
            switch (topic)
            {
                case "smarthome/smarthome_all":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on all devices
                    }
                    else
                    {
                        // Turn off all devices
                    }
                    break;
                case "lighting/parking_light":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        labelParkingLightSwitch.Text = "ON";
                        labelParkingLightSwitch.ForeColor = Color.Black;
                        richTextBoxParkingLight.BackColor = Color.Yellow;
                    }
                    else
                    {
                        labelParkingLightSwitch.Text = "OFF";
                        labelParkingLightSwitch.ForeColor = Color.White;
                        richTextBoxParkingLight.BackColor = Color.Black;
                    }
                    break;
                case "lighting/garden_light":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on garden light
                    }
                    else
                    {
                        // Turn off garden light
                    }
                    break;
                case "lighting/lighting_all":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on all lights
                    }
                    else
                    {
                        // Turn off all lights
                    }
                    break;
                case "heating/air_conditioning":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on air conditioning
                    }
                    else
                    {
                        // Turn off air conditioning
                    }
                    break;
                case "heating/heater":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on heater
                    }
                    else
                    {
                        // Turn off heater
                    }
                    break;
                case "heating/heating_all":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on all heating devices
                    }
                    else
                    {
                        // Turn off all heating devices
                    }
                    break;
                case "security/camera":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on camera
                    }
                    else
                    {
                        // Turn off camera
                    }
                    break;
                case "security/alarm":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on alarm
                    }
                    else
                    {
                        // Turn off alarm
                    }
                    break;
                case "security/security_all":
                    if (mqttMessage.Record.Content == "ON")
                    {
                        // Turn on all security devices
                    }
                    else
                    {
                        // Turn off all security devices
                    }
                    break;
                default:
                    break;
            }
        }

        private void AppendMqttMessage(MqttMessage mqttMessage)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);

            try
            {
                XDocument doc;

                if (File.Exists(filePath))
                {
                    doc = XDocument.Load(filePath);
                }
                else
                {
                    doc = new XDocument(new XElement("MqttMessages"));
                }

                XElement messageElement = new XElement("MqttMessage",
                    new XElement("Topic", mqttMessage.Topic),
                    new XElement("Event", mqttMessage.Event),
                    new XElement("Record",
                        new XElement("Id", mqttMessage.Record.Id),
                        new XElement("Name", mqttMessage.Record.Name),
                        new XElement("CreationDateTime", mqttMessage.Record.CreationDateTime.ToString("o")),
                        new XElement("Parent", mqttMessage.Record.Parent),
                        new XElement("Content", mqttMessage.Record.Content)
                    )
                );
                doc.Root.Add(messageElement);
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
