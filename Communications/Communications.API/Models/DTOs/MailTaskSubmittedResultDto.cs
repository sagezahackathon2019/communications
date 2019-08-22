using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Models.DTOs
{
    public class MailTaskSubmittedResultDto
    {
        public string ReceivedTimestamp { get; set; }
        public bool Processed { get; set; }
        public string StatusUrl { get; set; }
        public string CancelUrl { get; set; }
        public string TaskId { get; set; }
        public string VendorId { get; set; }
    }
}
