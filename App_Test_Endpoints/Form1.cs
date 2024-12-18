/**
* @brief Somiod - Projeto de Integração de Sistemas
* @date 2024-12-18
* @authors Diogo Abegão, João Parreira, Marcelo Oliveira, Pedro Barbeiro
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace App_Test_Endpoints
{
    public partial class Form1 : Form
    {
        string baseURI = "http://localhost:51967/api/somiod/";
        public Form1()
        {
            InitializeComponent();
            // Set the base URI in the text box
            textBoxBaseURI.Text = baseURI;
        }

        // resetFields() clears all text boxes and resets their background colors
        private void resetFields()
        {
            textBoxBaseURI.Text = "";
            textBoxURI.Text = "";
            textBoxHeader.Text = "";
            textBoxHttpCode.Text = "";
            textBoxHttpCodeText.Text = "";
            textBoxHttpCode.BackColor = SystemColors.Control;
            textBoxHttpCodeText.BackColor = SystemColors.Control;
        }

        // getHttpResponseCodeText() returns the text of the HTTP response code
        private string getHttpResponseCodeText(int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    textBoxHttpCode.BackColor = Color.PaleGreen;
                    textBoxHttpCodeText.BackColor = Color.PaleGreen;
                    return "OK";
                case 201:
                    textBoxHttpCode.BackColor = Color.PaleGreen;
                    textBoxHttpCodeText.BackColor = Color.PaleGreen;
                    return "Created";
                case 204:
                    textBoxHttpCode.BackColor = Color.PaleGreen;
                    textBoxHttpCodeText.BackColor = Color.PaleGreen;
                    return "No Content";
                case 400:
                    textBoxHttpCode.BackColor = Color.LightSalmon;
                    textBoxHttpCodeText.BackColor = Color.LightSalmon;
                    return "Bad Request";
                case 403:
                    textBoxHttpCode.BackColor = Color.LightSalmon;
                    textBoxHttpCodeText.BackColor = Color.LightSalmon;
                    return "Forbidden";
                case 404:
                    textBoxHttpCode.BackColor = Color.LightSalmon;
                    textBoxHttpCodeText.BackColor = Color.LightSalmon;
                    return "Not Found";
                case 500:
                    textBoxHttpCode.BackColor = Color.LightCoral;
                    textBoxHttpCodeText.BackColor = Color.LightCoral;
                    return "Internal Server Error";
                default:
                    textBoxHttpCode.BackColor = SystemColors.Control;
                    textBoxHttpCodeText.BackColor = SystemColors.Control;
                    return "Unknown";
            }
        }

        // buttonGetAllApps_Click() sends a GET request to the URI to retrieve all applications
        private void buttonGetAllApps_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    string fullURI = baseURI;
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = "";
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonGetOneApp_Click() sends a GET request to the URI to retrieve one application
        private void buttonGetOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string URI = application;
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonGetAllContainersInOneApp_Click() sends a GET request to the URI to retrieve all containers in one application
        private void buttonGetAllContainersInOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string URI = application + "/" + "container";
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonGetOneContainerInOneApp_Click() sends a GET request to the URI to retrieve one container in one application
        private void buttonGetOneContainerInOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container;
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonGetAllNotifsInOneContainer_Click() sends a GET request to the URI to retrieve all notifications in one container
        private void buttonGetAllNotifsInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container + "/notif";
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonOneNotifInOneContainer_Click() sends a GET request to the URI to retrieve one notification in one container
        private void buttonOneNotifInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxNotification.Text == "")
                    {
                        MessageBox.Show("Please enter a notification name.");
                        return;
                    }
                    if (textBoxNotification.Text.Contains("/"))
                    {
                        MessageBox.Show("Notification name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string notification = textBoxNotification.Text;
                    string URI = application + "/" + container + "/notif/" + notification;
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonGetAllRecordsInOneContainer_Click() sends a GET request to the URI to retrieve all records in one container
        private void buttonGetAllRecordsInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container + "/record";
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonGetOneRecordInOneContainer_Click() sends a GET request to the URI to retrieve one record in one container
        private void buttonGetOneRecordInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxRecord.Text == "")
                    {
                        MessageBox.Show("Please enter a record name.");
                        return;
                    }
                    if (textBoxRecord.Text.Contains("/"))
                    {
                        MessageBox.Show("Record name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string record = textBoxRecord.Text;
                    string URI = application + "/" + container + "/record/" + record;
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonPostOneApp_Click() sends a POST request to the URI to create one application
        private void buttonPostOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    string fullURI = baseURI;
                    string requestBody = textBoxRequestBody.Text.Trim();
                    HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PostAsync(fullURI, content).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = "";
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (XmlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonPostOneContainerInOneApp_Click() sends a POST request to the URI to create one container in one application
        private void buttonPostOneContainerInOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string URI = application;
                    string fullURI = baseURI + URI;
                    string requestBody = textBoxRequestBody.Text.Trim();
                    HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PostAsync(fullURI, content).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (XmlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonPostOneNotifInOneContainer_Click() sends a POST request to the URI to create one notification in one container
        private void buttonPostOneNotifInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container;
                    string fullURI = baseURI + URI;
                    string header = "res_type";
                    string headerValue = "notification";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    string requestBody = textBoxRequestBody.Text.Trim();
                    HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PostAsync(fullURI, content).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonPostOneRecordInOneContainer_Click() sends a POST request to the URI to create one record in one container
        private void buttonPostOneRecordInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container;
                    string fullURI = baseURI + URI;
                    string header = "res_type";
                    string headerValue = "record";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    string requestBody = textBoxRequestBody.Text.Trim();
                    HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PostAsync(fullURI, content).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonDeleteOneApp_Click() sends a DELETE request to the URI to delete one application
        private void buttonDeleteOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string URI = application;
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.DeleteAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonDeleteOneContainerInOneApp_Click() sends a DELETE request to the URI to delete one container in one application
        private void buttonDeleteOneContainerInOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container;
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.DeleteAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonDeleteOneNotifInOneContainer_Click() sends a DELETE request to the URI to delete one notification in one container
        private void buttonDeleteOneNotifInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxNotification.Text == "")
                    {
                        MessageBox.Show("Please enter a notification name.");
                        return;
                    }
                    if (textBoxNotification.Text.Contains("/"))
                    {
                        MessageBox.Show("Notification name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string notification = textBoxNotification.Text;
                    string URI = application + "/" + container + "/notif/" + notification;
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.DeleteAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonDeleteOneRecordInOneContainer_Click() sends a DELETE request to the URI to delete one record in one container
        private void buttonDeleteOneRecordInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxRecord.Text == "")
                    {
                        MessageBox.Show("Please enter a record name.");
                        return;
                    }
                    if (textBoxRecord.Text.Contains("/"))
                    {
                        MessageBox.Show("Record name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string record = textBoxRecord.Text;
                    string URI = application + "/" + container + "/record/" + record;
                    string fullURI = baseURI + URI;
                    HttpResponseMessage response = client.DeleteAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonPutOneApp_Click() sends a PUT request to the URI to update one application
        private void buttonPutOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string URI = application;
                    string fullURI = baseURI + URI;
                    string requestBody = textBoxRequestBody.Text.Trim();
                    HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PutAsync(fullURI, content).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (XmlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonPutOneContainerInOneApp_Click() sends a PUT request to the URI to update one container in one application
        private void buttonPutOneContainerInOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container;
                    string fullURI = baseURI + URI;
                    string requestBody = textBoxRequestBody.Text.Trim();
                    HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PutAsync(fullURI, content).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllApps_Click() sends a GET request to the URI to locate all application's names
        private void buttonLocateAllApps_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    string fullURI = baseURI;
                    string header = "somiod-locate";
                    string headerValue = "application";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = "";
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllContainers_Click() sends a GET request to the URI to locate all container's names
        private void buttonLocateAllContainers_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    string fullURI = baseURI;
                    string header = "somiod-locate";
                    string headerValue = "container";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = "";
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllNotifs_Click() sends a GET request to the URI to locate all notification's names
        private void buttonLocateAllNotifs_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    string fullURI = baseURI;
                    string header = "somiod-locate";
                    string headerValue = "notification";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = "";
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllRecords_Click() sends a GET request to the URI to locate all record's names
        private void buttonLocateAllRecords_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    string fullURI = baseURI;
                    string header = "somiod-locate";
                    string headerValue = "record";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = "";
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllContainersInOneApp_Click() sends a GET request to the URI to locate all container's names in one application
        private void buttonLocateAllContainersInOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string URI = application;
                    string fullURI = baseURI + URI;
                    string header = "somiod-locate";
                    string headerValue = "container";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllNotifsInOneApp_Click() sends a GET request to the URI to locate all notification's names in one application
        private void buttonLocateAllNotifsInOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string URI = application;
                    string fullURI = baseURI + URI;
                    string header = "somiod-locate";
                    string headerValue = "notification";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllRecordsInOneApp_Click() sends a GET request to the URI to locate all record's names in one application
        private void buttonLocateAllRecordsInOneApp_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string URI = application;
                    string fullURI = baseURI + URI;
                    string header = "somiod-locate";
                    string headerValue = "record";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllNotifsInOneContainer_Click() sends a GET request to the URI to locate all notification's names in one container
        private void buttonLocateAllNotifsInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container;
                    string fullURI = baseURI + URI;
                    string header = "somiod-locate";
                    string headerValue = "notification";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonLocateAllRecordsInOneContainer_Click() sends a GET request to the URI to locate all record's names in one container
        private void buttonLocateAllRecordsInOneContainer_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    resetFields();
                    if (textBoxApplication.Text == "")
                    {
                        MessageBox.Show("Please enter an application name.");
                        return;
                    }
                    if (textBoxApplication.Text.Contains("/"))
                    {
                        MessageBox.Show("Application name cannot contain a forward slash.");
                        return;
                    }
                    if (textBoxContainer.Text == "")
                    {
                        MessageBox.Show("Please enter a container name.");
                        return;
                    }
                    if (textBoxContainer.Text.Contains("/"))
                    {
                        MessageBox.Show("Container name cannot contain a forward slash.");
                        return;
                    }
                    string application = textBoxApplication.Text;
                    string container = textBoxContainer.Text;
                    string URI = application + "/" + container;
                    string fullURI = baseURI + URI;
                    string header = "somiod-locate";
                    string headerValue = "record";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxBaseURI.Text = baseURI;
                    textBoxURI.Text = URI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    textBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // buttonSaveBaseURI_Click() saves the base URI from the text box
        private void buttonSaveBaseURI_Click(object sender, EventArgs e)
        {
            baseURI = textBoxBaseURI.Text + "/";
        }
    }
}
