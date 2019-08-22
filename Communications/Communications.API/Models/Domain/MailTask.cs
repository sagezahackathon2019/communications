using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Models.Domain
{
    public class MailTask
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Processed { get; set; }
        public DateTime? ProcessedTimestamp { get; set; }
        public DateTime ReceivedTimestamp { get; set; }
        public Guid VendorId { get; set; }
        public string ClientId { get; set; }
    }
}
