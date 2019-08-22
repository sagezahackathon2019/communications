namespace Communications.API.Models.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class VendorDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DefaultFromAddress { get; set; }
        public string Name { get; set; }
    }
}