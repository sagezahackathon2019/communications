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

        public class Response
        {
            public MailTaskResult Payload { get; set; }
            public bool Success { get; set; }
        }

        public class MailTaskResult
        {
            public string ReceivedTimestamp { get; set; }
            public bool Processed { get; set; }
            public string StatusUrl { get; set; }
            public string CancelUrl { get; set; }
            public string TaskId { get; set; }
            public string VendorId { get; set; }
        }

        public void SendEmailTask(string baseUrl, string vendorKey, string clientId, string from, string to, string subject, string body)
        {
            BaseUrl = baseUrl.Trim();
            try
            {
                MailObject mailObject = new MailObject
                {
                    Body = body.Trim(),
                    ClientId = clientId.Trim(),
                    From = from.Trim(),
                    Subject = subject.Trim(),
                    To = to.Trim().Split(';')
                };

                var result = WebUtility.SendRequestJSON($"{BaseUrl.Trim()}/api/communications/mail/submit", "POST", vendorKey.Trim(), mailObject, null);
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