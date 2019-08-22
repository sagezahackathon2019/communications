using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Models.DTOs
{
    public class MailTaskStatusDto
    {
        public string ReceivedTimestamp { get; set; }
        public string VendorId { get; set; }
        public bool Processed { get; set; }
        public string ProcessedTimestamp { get; set; }
        public string TaskId { get; set; }
    }
}
