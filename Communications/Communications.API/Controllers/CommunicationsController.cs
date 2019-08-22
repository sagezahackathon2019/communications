using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Communications.API.Data;
using Communications.API.Helpers;
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

            var vendorId = HttpContext.Items.Where(x => x.Key == "VendorId").First().Value.ToString();

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
                VendorId = Guid.Parse(vendorId)
            };

            _context.MailTasks.Add(newMailTask);
            _context.SaveChanges();

            string taskStatusUrl = $"api/communications/mail/status/{newMailTask.Id}";
            string taskCancelUrl = $"api/communications/mail/cancel/{newMailTask.Id}";

            var nestedPayload = new MailTaskSubmittedResultDto()
            {
                CancelUrl = taskCancelUrl,
                StatusUrl = taskStatusUrl,
                Processed = newMailTask.Processed,
                ReceivedTimestamp = newMailTask.ReceivedTimestamp.ToString("yyyy-MM-dd hh:mm:ss fff"),
                TaskId = newMailTask.Id.ToString(),
                VendorId = newMailTask.VendorId.ToString()
            };

            Hangfire.BackgroundJob.Enqueue<MailHelper>(x => x.ProcessMailTask(newMailTask.Id));

            var payload = new ResponseObject<MailTaskSubmittedResultDto>()
            {
                Payload = nestedPayload,
                Success = true
            };

            return Accepted(uri: taskStatusUrl, value: payload);
        }


        [HttpGet]
        [Route("mail/status/{id}")]
        public IActionResult GetMailTaskStatus([FromRoute] string id)
        {
            Guid taskId = Guid.Parse(id);

            var mailTask = _context.MailTasks
                .Where(x => x.Id == taskId)
                .FirstOrDefault();

            var nestedPayload = new MailTaskStatusDto()
            {
                Processed = mailTask.Processed,
                TaskId = mailTask.Id.ToString(),
                VendorId = mailTask.VendorId.ToString(),
                ReceivedTimestamp = mailTask.ReceivedTimestamp.ToString("yyyy-MM-dd hh:mm:ss fff"),
                ProcessedTimestamp = mailTask.ProcessedTimestamp.HasValue ? mailTask.ProcessedTimestamp.Value.ToString("yyyy-MM-dd hh:mm:ss fff") : ""
            };

            var payload = new ResponseObject<MailTaskStatusDto>()
            {
                Payload = nestedPayload,
                Success = true
            };

            return Json(payload);
        }

    }
}