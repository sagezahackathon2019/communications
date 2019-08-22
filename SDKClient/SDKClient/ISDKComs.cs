using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKClient
{
    public interface ISDKComs
    {
        void SendEmailTask();

        void GetEmailTaskStatus(string taskId);

        void GetReportByClientID();
    }
}