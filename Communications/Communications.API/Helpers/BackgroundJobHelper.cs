using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace Communications.API.Helpers
{
    public class BackgroundJobHelper
    {
        public static void AddDefaultHangfireProcesses()
        {
            RecurringJob.AddOrUpdate<MailHelper>(x => x.CleanupEmails(), "0 0 1 * *");
        }
    }
}
