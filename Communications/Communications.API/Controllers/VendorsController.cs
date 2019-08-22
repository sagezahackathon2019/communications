namespace Communications.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Communications.API.Data;
    using Communications.API.Models;
    using Communications.API.Models.Domain;
    using Communications.API.Models.DTOs;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : Controller
    {
        private MainDbContext _context;
        private readonly IConfiguration _configuration;

        public VendorsController(IConfiguration configuration, MainDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(VendorDto newVendor)
        {
            var vendorAlreadyExists = _context.Vendors.Any(x => x.Name.ToLower() == newVendor.Name.ToLower());

            if (vendorAlreadyExists)
                return BadRequest("Vendor with that name already exists.");

            var vendor = new Vendor()
            {
                Active = true,
                DefaultFromAddress = newVendor.DefaultFromAddress,
                Name = newVendor.Name,
                RegisteredTimestamp = DateTime.Now
            };

            _context.Vendors.Add(vendor);
            _context.SaveChanges();

            var payload = new ResponseObject<string>()
            {
                Payload = vendor.Id.ToString(),
                Success = true
            };

            return Json(payload);
        }
    }
}