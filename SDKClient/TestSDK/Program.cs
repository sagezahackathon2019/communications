using SDKClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSDK
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string BaseURL = "http://172.20.51.51:44306";

            MailTaskSubmitter client = new MailTaskSubmitter();
            client.BaseUrl = BaseURL;
            string key = "8afde5db-627c-41e3-9c28-26237c31b726";
            string from = "jaco.venter@sage.com";
            string to = "masheleni@gmail.com";

            client.SendEmailTask(key, "EI001", from, to, "Test ME", "THIS IS ME");

            var success = client.Success;
        }
    }
}