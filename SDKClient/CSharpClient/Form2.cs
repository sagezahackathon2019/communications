using SDKClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpClient
{
    public partial class Form2 : Form
    {
        private string _taskID;
        private string _url = "http://172.20.51.51:44306";
        private string key = "8afde5db-627c-41e3-9c28-26237c31b726";

        public Form2()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text.Trim();
            string to = txtTo.Text.Trim();
            string subject = txtSubject.Text.Trim();
            string body = txtBody.Text;

            var mail = new MailTaskSubmitter();
            mail.BaseUrl = "http://172.20.51.51:44306";

            mail.SendEmailTask(_url, key, "EI002", from, to, subject, body);

            _taskID = mail.TaskId;
            lblTaskID.Text = _taskID;
        }

        private void BtnStatus_Click(object sender, EventArgs e)
        {
            var status = new MailTaskStatus();
            status.GetEmailTaskStatus(_url, _taskID, key);
            string result = $"Processed: {status.Processed}\r\nReceivedTimeStamp:{status.ReceivedTimestamp}\r\nProcessedTimeStamp:{status.ProcessedTimestamp}";
            textBox1.Text = result;
        }
    }
}