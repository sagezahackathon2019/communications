namespace Communications.API.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    using Communications.API.Data;
    using Communications.API.Models;
    using Communications.API.Models.DTOs;
    using Communications.API.Models.Domain;

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
            var vendorId = HttpContext.Items.Where(x => x.Key == "VendorId").First().Value.ToString();

            Guid taskId;

            var parseResult = Guid.TryParse(vendorId, out taskId);

            if (parseResult)
            {
                var mailTask = _context.MailTasks
                .Where(x => x.Id == taskId && x.VendorId == Guid.Parse(vendorId))
                .FirstOrDefault();

                if (mailTask != null)
                {
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
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest("The TaskId provided isn't a valid Guid.");
            }
        }

        [HttpGet]
        [Route("mail/cancel/{id}")]
        public IActionResult CancelMail([FromRoute] string id)
        {
            var vendorId = HttpContext.Items.Where(x => x.Key == "VendorId").First().Value.ToString();

            Guid taskId;

            var parseResult = Guid.TryParse(vendorId, out taskId);

            if (parseResult)
            {
                var mailTask = _context.MailTasks
                .Where(x => x.Id == taskId && x.VendorId == Guid.Parse(vendorId))
                .FirstOrDefault();

                if (mailTask != null)
                {
                    if (!mailTask.Processed)
                    {
                        mailTask.Processed = true;
                        _context.SaveChanges();

                        var nestedPayload = new MailTaskCancelledStatusDto()
                        {
                            TaskId = mailTask.Id.ToString(),
                            CancelledTimestamp = DateTime.Now
                        };

                        var payload = new ResponseObject<MailTaskCancelledStatusDto>()
                        {
                            Payload = nestedPayload,
                            Success = true
                        };

                        return Json(payload);
                    }
                    else
                    {
                        var payload = new ResponseObject<string>()
                        {
                            Payload = "Mail has already been processed.",
                            Success = true
                        };

                        return Json(payload);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest("The TaskId provided isn't a valid Guid.");
            }
        }

        [HttpGet]
        [Route("mail/status/all")]
        public IActionResult GetMailTasksStatusesForVendorAndClient([FromQuery] string vendorId, [FromQuery] string clientId)
        {
            Guid parsedGuid;

            var parseResult = Guid.TryParse(vendorId, out parsedGuid);

            if (parseResult)
            {
                var mailTasks = _context.MailTasks
                .Where(x => x.VendorId == Guid.Parse(vendorId) && x.ClientId == clientId)
                .Select(x => new MailTaskStatusDto()
                {
                    Processed = x.Processed,
                    ProcessedTimestamp = x.ProcessedTimestamp.HasValue ? x.ProcessedTimestamp.Value.ToString("yyyy-MM-dd hh:mm:ss fff") : "",
                    ReceivedTimestamp = x.ReceivedTimestamp.ToString("yyyy-MM-dd hh:mm:ss fff"),
                    TaskId = x.Id.ToString(),
                    VendorId = x.VendorId.ToString()
                }).ToList();

                var payload = new ResponseObject<IEnumerable<MailTaskStatusDto>>()
                {
                    Success = true,
                    Payload = mailTasks
                };

                return Json(payload);
            }
            else
            {
                return BadRequest("The vendorId provided isn't a valid Guid.");
            }
        }

        [HttpGet]
        [Route("mail/status/processed")]
        public IActionResult GetProcessedMailTaskStatusesForVendorAndClient([FromQuery] string vendorId, [FromQuery] string clientId)
        {
            Guid parsedGuid;

            var parsedResult = Guid.TryParse(vendorId, out parsedGuid);

            if (parsedResult)
            {
                var mailTasks = _context.MailTasks
                    .Where(x => x.VendorId == Guid.Parse(vendorId) && x.ClientId == clientId && x.Processed)
                    .Select(x => new MailTaskStatusDto()
                    {
                        Processed = x.Processed,
                        ProcessedTimestamp = x.ProcessedTimestamp.HasValue ? x.ProcessedTimestamp.Value.ToString("yyyy-MM-dd hh:mm:ss fff") : "",
                        ReceivedTimestamp = x.ReceivedTimestamp.ToString("yyyy-MM-dd hh:mm:ss fff"),
                        TaskId = x.Id.ToString(),
                        VendorId = x.VendorId.ToString()
                    }).ToList();

                var payload = new ResponseObject<IEnumerable<MailTaskStatusDto>>()
                {
                    Success = true,
                    Payload = mailTasks
                };

                return Json(payload);
            }
            else
            {
                return BadRequest("The vendorId provided isn't a valid Guid.");
            }
        }

        [HttpGet]
        [Route("mail/status/unprocessed")]
        public IActionResult GetUnprocessedMailTaskStatusesForVendorAndClient([FromQuery] string vendorId, [FromQuery] string clientId)
        {
            Guid parsedGuid;

            var parsedResult = Guid.TryParse(vendorId, out parsedGuid);

            if (parsedResult)
            {
                var mailTasks = _context.MailTasks
                .Where(x => x.VendorId == Guid.Parse(vendorId) && x.ClientId == clientId && !x.Processed)
                .Select(x => new MailTaskStatusDto()
                {
                    Processed = x.Processed,
                    ProcessedTimestamp = x.ProcessedTimestamp.HasValue ? x.ProcessedTimestamp.Value.ToString("yyyy-MM-dd hh:mm:ss fff") : "",
                    ReceivedTimestamp = x.ReceivedTimestamp.ToString("yyyy-MM-dd hh:mm:ss fff"),
                    TaskId = x.Id.ToString(),
                    VendorId = x.VendorId.ToString()
                }).ToList();

                var payload = new ResponseObject<IEnumerable<MailTaskStatusDto>>()
                {
                    Success = true,
                    Payload = mailTasks
                };

                return Json(payload);
            }
            else
            {
                return BadRequest("The vendorId provided isn't a valid Guid.");
            }
        }
    }
}