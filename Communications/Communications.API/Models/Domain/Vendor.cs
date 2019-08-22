using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Models.Domain
{
    public class Vendor
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DefaultFromAddress { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime RegisteredTimestamp { get; set; }
    }
}