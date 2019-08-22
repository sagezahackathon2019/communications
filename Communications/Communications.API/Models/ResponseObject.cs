using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Models
{
    public class ResponseObject<T>
    {
        public T Payload { get; set; }
        public bool Success { get; set; }
    }
}
