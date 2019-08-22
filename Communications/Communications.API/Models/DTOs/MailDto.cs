using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Models.DTOs
{
    public class MailDto
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] To { get; set; }
        public string ClientId { get; set; }
    }
}
