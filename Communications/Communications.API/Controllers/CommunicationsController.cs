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

namespace Communications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationsController : Controller
    {
        #region Fields

        private MainDbContext _context;

        #endregion Fields

        #region Constructors

        public CommunicationsController(MainDbContext context)
        {
            this._context = context;
        }

        #endregion Constructors

        [HttpPost]
        [Route("mail/submit")]
        public IActionResult SubmitMail([FromBody] MailDto mailDto)
        {
            _context.Database.EnsureCreated();

            var newMailTask = new MailTask()
            {
                From = mailDto.From,
                To = string.Join(';', mailDto.To),
                Body = mailDto.Body,
                Processed = false,
                ProcessedTimestamp = null,
                ReceivedTimestamp = DateTime.Now,
                Subject = mailDto.Subject,
                ClientId = mailDto.ClientId,
                VendorId = Guid.NewGuid()
            };

            _context.MailTasks.Add(newMailTask);
            _context.SaveChanges();

            string taskStatusUrl = $"api/communications/mail/status/{newMailTask.Id}";
            string taskCancelUrl = $"api/communications/mail/cancel/{newMailTask.Id}";

            var nestedPayload = new
            {
                ReceivedTimestamp = newMailTask.ReceivedTimestamp.ToString("yyyy-MM-dd hh:mm:ss fff"),
                Processed = newMailTask.Processed,
                StatusUrl = taskStatusUrl,
                CancelUrl = taskCancelUrl,
                TaskId = newMailTask.Id,
                VendorKey = newMailTask.VendorId
            };

            var payload = new ResponseObject<object>()
            {
                Payload = nestedPayload,
                Success = true
            };

            return Accepted(uri: taskStatusUrl, value: payload);
        }


    }
}