using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Models.DTOs
{
    public class MailTaskCancelledStatusDto
    {
        public string TaskId { get; set; }
        public DateTime CancelledTimestamp { get; set; }
    }
}
