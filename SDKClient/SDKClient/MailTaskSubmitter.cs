using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKClient
{
    public class MailTaskSubmitter
    {
        public string BaseUrl { get; set; }
        public string ReceivedTimestamp { get; set; }
        public bool Processed { get; set; }
        public string StatusUrl { get; set; }
        public string CancelUrl { get; set; }
        public string TaskId { get; set; }
        public string VendorId { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public int Success { get; set; }

        internal class Response
        {
            internal MailTaskResult Payload { get; set; }
            internal bool Success { get; set; }
        }

        internal class MailTaskResult
        {
            internal string ReceivedTimestamp { get; set; }
            internal bool Processed { get; set; }
            internal string StatusUrl { get; set; }
            internal string CancelUrl { get; set; }
            internal string TaskId { get; set; }
            internal string VendorId { get; set; }
        }

        public void SendEmailTask(string vendorKey, string clientId, string from, string to, string subject, string body)
        {
            try
            {
                MailObject mailObject = new MailObject
                {
                    Body = body,
                    ClientId = clientId,
                    From = from,
                    Subject = subject,
                    To = to.Split(';')
                };

                var result = WebUtility.SendRequestJSON($"{BaseUrl}/api/communications/mail/submit", "POST", vendorKey, mailObject, null);
                var response = JsonConvert.DeserializeObject<Response>(result);

                Success = response.Success ? 1 : 0;

                if (!response.Success)
                {
                    return;
                }

                var mailTaskResult = response.Payload;

                ReceivedTimestamp = mailTaskResult.ReceivedTimestamp;
                Processed = mailTaskResult.Processed;
                CancelUrl = mailTaskResult.CancelUrl;
                StatusUrl = mailTaskResult.StatusUrl;
                TaskId = mailTaskResult.TaskId;
                VendorId = mailTaskResult.VendorId;
                return;
            }
            catch (Exception exc)
            {
                Success = 0;
                ErrorCode = -1;
                ErrorMessage = exc.Message;
            }

            return;
        }
    }
}