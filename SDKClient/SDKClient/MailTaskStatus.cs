using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKClient
{
    public class MailTaskStatus
    {
        public string BaseUrl { get; set; }
        public string ReceivedTimestamp { get; set; }
        public string VendorId { get; set; }
        public bool Processed { get; set; }
        public string ProcessedTimestamp { get; set; }
        public string TaskId { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public int Success { get; set; }
        public int RecordIndex = -1;

        public List<MailTaskStatusDTO> MailTaskList { get; set; }

        public class SingleResponse
        {
            public MailTaskStatusDTO Payload { get; set; }
            public bool Success { get; set; }
        }

        public class ListResponse
        {
            public List<MailTaskStatusDTO> Payload { get; set; }
            public bool Success { get; set; }
        }

        public class MailTaskStatusDTO
        {
            public string ReceivedTimestamp { get; set; }
            public string VendorId { get; set; }
            public bool Processed { get; set; }
            public string ProcessedTimestamp { get; set; }
            public string TaskId { get; set; }
        }

        public void GetEmailTaskStatus(string baseUrl, string taskId, string vendorKey)
        {
            BaseUrl = baseUrl;
            RecordIndex = -1;
            try
            {
                string result = WebUtility.SendRequestJSON($"{BaseUrl}/api/communications/mail/status/{taskId}", "GET", vendorKey, null, null);
                var response = JsonConvert.DeserializeObject<SingleResponse>(result);
                Success = response.Success ? 1 : 0;

                if (!response.Success)
                {
                    return;
                }

                var mailTaskResult = response.Payload;
                Processed = mailTaskResult.Processed;
                ProcessedTimestamp = mailTaskResult.ProcessedTimestamp;
                ReceivedTimestamp = mailTaskResult.ReceivedTimestamp;
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

        public void NextRecord()
        {
            if (RecordIndex > -1)
                RecordIndex++;

            if (RecordIndex < MailTaskList.Count() && RecordIndex > -1)
            {
                var mailTaskResult = MailTaskList[RecordIndex];
                Processed = mailTaskResult.Processed;
                ProcessedTimestamp = mailTaskResult.ProcessedTimestamp;
                ReceivedTimestamp = mailTaskResult.ReceivedTimestamp;
                TaskId = mailTaskResult.TaskId;
                VendorId = mailTaskResult.VendorId;
            }

            if (RecordIndex >= MailTaskList.Count())
                RecordIndex = -1;
        }

        public void GetAllEmailTasksForClient(string baseUrl, string vendorKey, string clientId)
        {
            BaseUrl = baseUrl.Trim();
            try
            {
                string result = WebUtility.SendRequestJSON($"{BaseUrl}/api/communications/mail/status/all?vendorId={vendorKey}&clientId={clientId}", "GET", vendorKey, null, null);
                var response = JsonConvert.DeserializeObject<ListResponse>(result);
                Success = response.Success ? 1 : 0;

                if (!response.Success)
                {
                    return;
                }

                var mailTaskResult = response.Payload;
                if (mailTaskResult.Count == 0)
                {
                    RecordIndex = -1;
                    return;
                }

                MailTaskList = mailTaskResult;
                RecordIndex = 0;
                var mailTask = MailTaskList[RecordIndex];
                Processed = mailTask.Processed;
                ProcessedTimestamp = mailTask.ProcessedTimestamp;
                ReceivedTimestamp = mailTask.ReceivedTimestamp;
                TaskId = mailTask.TaskId;
                VendorId = mailTask.VendorId;

                return;
            }
            catch (Exception exc)
            {
                RecordIndex = -1;
                Success = 0;
                ErrorCode = -1;
                ErrorMessage = exc.Message;
            }

            return;
        }

        public void GetUnprocessedEmailTasksForClient(string baseUrl, string vendorKey, string clientId)
        {
            BaseUrl = baseUrl.Trim();
            try
            {
                string result = WebUtility.SendRequestJSON($"{BaseUrl}/api/communications/mail/status/unprocessed?vendorId={vendorKey}&clientId={clientId}", "GET", vendorKey, null, null);
                var response = JsonConvert.DeserializeObject<ListResponse>(result);
                Success = response.Success ? 1 : 0;

                if (!response.Success)
                {
                    return;
                }

                var mailTaskResult = response.Payload;
                if (mailTaskResult.Count == 0)
                {
                    RecordIndex = -1;
                    return;
                }

                MailTaskList = mailTaskResult;
                RecordIndex = 0;
                var mailTask = MailTaskList[RecordIndex];
                Processed = mailTask.Processed;
                ProcessedTimestamp = mailTask.ProcessedTimestamp;
                ReceivedTimestamp = mailTask.ReceivedTimestamp;
                TaskId = mailTask.TaskId;
                VendorId = mailTask.VendorId;

                return;
            }
            catch (Exception exc)
            {
                RecordIndex = -1;
                Success = 0;
                ErrorCode = -1;
                ErrorMessage = exc.Message;
            }

            return;
        }

        public void GetProcessedEmailTasksForClient(string baseUrl, string vendorKey, string clientId)
        {
            BaseUrl = baseUrl.Trim();
            try
            {
                string result = WebUtility.SendRequestJSON($"{BaseUrl}/api/communications/mail/status/processed?vendorId={vendorKey}&clientId={clientId}", "GET", vendorKey, null, null);
                var response = JsonConvert.DeserializeObject<ListResponse>(result);
                Success = response.Success ? 1 : 0;

                if (!response.Success)
                {
                    return;
                }

                var mailTaskResult = response.Payload;
                if (mailTaskResult.Count == 0)
                {
                    RecordIndex = -1;
                    return;
                }

                MailTaskList = mailTaskResult;
                RecordIndex = 0;
                var mailTask = MailTaskList[RecordIndex];
                Processed = mailTask.Processed;
                ProcessedTimestamp = mailTask.ProcessedTimestamp;
                ReceivedTimestamp = mailTask.ReceivedTimestamp;
                TaskId = mailTask.TaskId;
                VendorId = mailTask.VendorId;

                return;
            }
            catch (Exception exc)
            {
                RecordIndex = -1;
                Success = 0;
                ErrorCode = -1;
                ErrorMessage = exc.Message;
            }

            return;
        }
    }
}