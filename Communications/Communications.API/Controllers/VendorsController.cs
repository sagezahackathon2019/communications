namespace Communications.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Communications.API.Models.DTOs;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VendorsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("Register")]
        public VendorDto Register(VendorDto newVendor)
        {
            newVendor.ID = Guid.NewGuid();
            return newVendor;
        }
    }
}