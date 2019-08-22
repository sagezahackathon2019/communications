using Newtonsoft.Json;
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
    public partial class Form1 : Form
    {
        private string _url = "http://172.20.51.51:44306";
        private string _key = "8afde5db-627c-41e3-9c28-26237c31b726";

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnNewMail_Click(object sender, EventArgs e)
        {
            var newEmail = new Form2();
            newEmail.ShowDialog();
        }

        private void BtnAll_Click(object sender, EventArgs e)
        {
            var status = new MailTaskStatus();
            status.GetAllEmailTasksForClient(_url, _key, "EI002");

            textBox1.Text = JsonConvert.SerializeObject(status.MailTaskList);
        }

        private void BtnPending_Click(object sender, EventArgs e)
        {
            var status = new MailTaskStatus();
            status.GetUnprocessedEmailTasksForClient(_url, _key, "EI002");

            textBox1.Text = JsonConvert.SerializeObject(status.MailTaskList);
        }

        private void BtnSent_Click(object sender, EventArgs e)
        {
            var status = new MailTaskStatus();
            status.GetProcessedEmailTasksForClient(_url, _key, "EI002");

            textBox1.Text = JsonConvert.SerializeObject(status.MailTaskList);
        }
    }
}