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
            ToggleAll(false);
        }

        private async void InitializeSmartHomeAsync()
        {
            try
            {
                string[] applications = { "smarthome", "lighting", "heating", "security" };
                (string application, string container)[] containersAndNotifications =
                {
                    ("smarthome", "smarthome_all"),
                    ("lighting", "parking_light"),
                    ("lighting", "garden_light"),
                    ("lighting", "lighting_all"),
                    ("heating", "air_conditioning"),
                    ("heating", "heater"),
                    ("heating", "heating_all"),
                    ("security", "camera"),
                    ("security", "alarm"),
                    ("security", "security_all")
                };
                foreach (var app in applications)
                {
                    await CreateApplicationAsync(app);
                }
                foreach (var (application, container) in containersAndNotifications)
                {
                    await CreateContainerAsync(application, container);
                }
                foreach (var (application, container) in containersAndNotifications)
                {
                    await CreateNotification(application, container);
                }
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

        private async Task CreateContainerAsync(string applicationName, string containerName)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Container cont = new Container
                    {
                        id = 0,
                        name = containerName,
                        creation_datetime = DateTime.Now,
                        parent = 0
                    };
                    string fullURI = baseURI + applicationName + "/" + containerName;
                    HttpResponseMessage response_1 = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response_1.StatusCode;
                    if (statusCode == 404)
                    {
                        fullURI = baseURI + applicationName;
                        string requestBody = XMLHelper.SerializeXmlUtf8<Container>(cont).ToString().Trim();
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

        private async Task CreateNotification(string applicationName, string containerName)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Notification not = new Notification
                    {
                        id = 0,
                        name = "not",
                        creation_datetime = DateTime.Now,
                        parent = 0,
                        event_type = "1",
                        endpoint = "127.0.0.1",
                        enabled = true
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
            DateTime receivedTime = DateTime.Now;
            string topic = e.Topic;
            string body = Encoding.UTF8.GetString(e.Message);
            string[] vars = body.Split(';');
            string mqttEvent = vars[0];
            string message = vars[1];
            HandleEvent(topic, mqttEvent, message, receivedTime);
            AppendMqttMessage(topic, mqttEvent, message, receivedTime);
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
                mClient.Subscribe(topics, qosLevels);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HandleEvent(string topic, string mqttEvent, string message, DateTime receivedTime)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleEvent(topic, mqttEvent, message, receivedTime)));
                return;
            }
            switch (topic)
            {
                case "smarthome/smarthome_all":
                    if (message == "ON")
                    {
                        ToggleAll(true);
                    }
                    else
                    {
                        ToggleAll(false);
                    }
                    break;
                case "lighting/parking_light":
                    if (message == "ON")
                    {
                        ToggleParkingLight(true);
                    }
                    else
                    {
                        ToggleParkingLight(false);
                    }
                    break;
                case "lighting/garden_light":
                    if (message == "ON")
                    {
                        ToggleGardenLight(true);
                    }
                    else
                    {
                        ToggleGardenLight(false);
                    }
                    break;
                case "lighting/lighting_all":
                    if (message == "ON")
                    {
                        ToggleLighting(true);
                    }
                    else
                    {
                        ToggleLighting(false);
                    }
                    break;
                case "heating/air_conditioning":
                    if (message == "ON")
                    {
                        ToggleAirConditioning(true);
                    }
                    else
                    {
                        ToggleAirConditioning(false);
                    }
                    break;
                case "heating/heater":
                    if (message == "ON")
                    {
                        ToggleAirConditioning(true);
                    }
                    else
                    {
                        ToggleAirConditioning(false);
                    }
                    break;
                case "heating/heating_all":
                    if (message == "ON")
                    {
                        ToggleHeating(true);
                    }
                    else
                    {
                        ToggleHeating(false);
                    }
                    break;
                case "security/camera":
                    if (message == "ON")
                    {
                        ToggleCamera(true);
                    }
                    else
                    {
                        ToggleCamera(false);    
                    }
                    break;
                case "security/alarm":
                    if (message == "ON")
                    {
                        ToggleAlarm(true);
                    }
                    else
                    {
                        ToggleAlarm(false);
                    }
                    break;
                case "security/security_all":
                    if (message == "ON")
                    {
                        ToggleSecurity(true);
                    }
                    else
                    {
                        ToggleSecurity(false);
                    }
                    break;
                default:
                    break;
            }
        }

        private void AppendMqttMessage(string topic, string eventType, string message, DateTime receivedTime)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            try
            {
                MqttMessage mqttMessage = new MqttMessage
                {
                    topic = topic,
                    event_type = eventType,
                    message = message,
                    received_time = receivedTime
                };
                string xmlMessage = XMLHelper.SerializeXmlUtf8<MqttMessage>(mqttMessage).ToString().Trim();
                XDocument doc;
                if (File.Exists(filePath))
                {
                    doc = XDocument.Load(filePath);
                }
                else
                {
                    doc = new XDocument(new XElement("MqttMessages"));
                }
                XElement messageElement = XElement.Parse(xmlMessage);
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
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            try
            {
                if (!File.Exists(filePath))
                {
                    return;
                }
                XDocument doc = XDocument.Load(filePath);
                DateTime cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                doc.Root.Elements("MqttMessage")
                    .Where(m =>
                    {
                        DateTime messageDate = DateTime.Parse(m.Element("received_time").Value);
                        return messageDate < cutoffDate;
                    })
                    .Remove();
                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error pruning old messages: {ex.Message}");
            }
        }

        private void ToggleParkingLight(bool status)
        {
            if (status)
            {
                labelParkingLightSwitch.Text = "ON";
                richTextBoxParkingLight.BackColor = Color.Yellow;
            }
            else
            {
                labelParkingLightSwitch.Text = "OFF";
                richTextBoxParkingLight.BackColor = Color.Black;
            }
        }

        private void ToggleGardenLight(bool status)
        {
            if (status)
            {
                labelGardenLightSwitch.Text = "ON";
                richTextBoxGardenLight.BackColor = Color.Yellow;
            }
            else
            {
                labelGardenLightSwitch.Text = "OFF";
                richTextBoxGardenLight.BackColor = Color.Black;
            }
        }

        private void ToggleAirConditioning(bool status)
        {
            if (status)
            {
                labelAirConditioningSwitch.Text = "ON";
                richTextBoxAirConditioning.BackColor = Color.Blue;
            }
            else
            {
                labelAirConditioningSwitch.Text = "OFF";
                richTextBoxAirConditioning.BackColor = Color.Black;
            }
        }

        private void ToggleHeater(bool status)
        {
            if (status)
            {
                labelHeaterSwitch.Text = "ON";
                richTextBoxHeater.BackColor = Color.Red;
            }
            else
            {
                labelHeaterSwitch.Text = "OFF";
                richTextBoxHeater.BackColor = Color.Black;
            }
        }

        private void ToggleCamera(bool status)
        {
            if (status)
            {
                labelCameraSwitch.Text = "ON";
                richTextBoxCamera.BackColor = Color.Green;
            }
            else
            {
                labelCameraSwitch.Text = "OFF";
                richTextBoxCamera.BackColor = Color.Black;
            }
        }

        private void ToggleAlarm(bool status)
        {
            if (status)
            {
                labelAlarmSwitch.Text = "ON";
                richTextBoxAlarm.BackColor = Color.Red;
            }
            else
            {
                labelAlarmSwitch.Text = "OFF";
                richTextBoxAlarm.BackColor = Color.Black;
            }
        }

        private void ToggleLighting(bool status)
        {
            if (status)
            {
                ToggleParkingLight(true);
                ToggleGardenLight(true);
            }
            else
            {
                ToggleParkingLight(false);
                ToggleGardenLight(false);
            }
        }

        private void ToggleHeating(bool status)
        {
            if (status)
            {
                ToggleAirConditioning(true);
                ToggleHeater(true);
            }
            else
            {
                ToggleAirConditioning(false);
                ToggleHeater(false);
            }
        }

        private void ToggleSecurity(bool status)
        {
            if (status)
            {
                ToggleCamera(true);
                ToggleAlarm(true);
            }
            else
            {
                ToggleCamera(false);
                ToggleAlarm(false);
            }
        }

        private void ToggleAll(bool status)
        {
            if (status)
            {
                ToggleLighting(true);
                ToggleHeating(true);
                ToggleSecurity(true);
            }
            else
            {
                ToggleLighting(false);
                ToggleHeating(false);
                ToggleSecurity(false);
            }
        }
    }
}
