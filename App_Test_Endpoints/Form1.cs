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

namespace App_Test_Endpoints
{
    public partial class Form1 : Form
    {
        string baseURI = "http://localhost:51897/api/somiod/";
        public Form1()
        {
            InitializeComponent();
        }

        private void resetFields()
        {
            textBoxURI.Text = "";
            textBoxHeader.Text = "";
            textBoxHttpCode.Text = "";
            textBoxHttpCodeText.Text = "";
            richTextBoxRequestBody.Text = "";
            richTextBoxResponseBody.Text = "";
            textBoxHttpCode.BackColor = SystemColors.Control;
            textBoxHttpCodeText.BackColor = SystemColors.Control;
        }

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
                    textBoxURI.Text = fullURI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

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
                    string fullURI = baseURI + application + "/";
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxURI.Text = fullURI;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonGetAllContainersInOneApp_Click(object sender, EventArgs e)
        {

        }

        private void buttonGetOneContainerInOneApp_Click(object sender, EventArgs e)
        {

        }

        private void buttonGetAllNotifsInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonOneNotifInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonGetAllRecordsInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonGetOneRecordInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonPostOneApp_Click(object sender, EventArgs e)
        {

        }

        private void buttonPostOneContainerInOneApp_Click(object sender, EventArgs e)
        {

        }

        private void buttonPostOneNotifInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonPostOneRecordInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteOneApp_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteOneContainerInOneApp_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteOneNotifInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteOneRecordInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonPutOneApp_Click(object sender, EventArgs e)
        {

        }

        private void buttonPutOneContainerInOneApp_Click(object sender, EventArgs e)
        {

        }

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
                    textBoxURI.Text = fullURI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

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
                    textBoxURI.Text = fullURI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

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
                    textBoxURI.Text = fullURI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

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
                    textBoxURI.Text = fullURI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

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
                    string fullURI = baseURI + application + "/";
                    string header = "somiod-locate";
                    string headerValue = "container";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxURI.Text = fullURI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

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
                    string fullURI = baseURI + application + "/";
                    string header = "somiod-locate";
                    string headerValue = "notification";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxURI.Text = fullURI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

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
                    string fullURI = baseURI + application + "/";
                    string header = "somiod-locate";
                    string headerValue = "record";
                    client.DefaultRequestHeaders.Add(header, headerValue);
                    HttpResponseMessage response = client.GetAsync(fullURI).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    textBoxURI.Text = fullURI;
                    textBoxHeader.Text = header + ": " + headerValue;
                    textBoxHttpCode.Text = statusCode.ToString();
                    textBoxHttpCodeText.Text = getHttpResponseCodeText(statusCode);
                    richTextBoxResponseBody.Text = responseBody;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonLocateAllNotifsInOneContainer_Click(object sender, EventArgs e)
        {

        }

        private void buttonLocateAllRecordsInOneContainer_Click(object sender, EventArgs e)
        {

        }
    }
}
