using Communications.API.Data;
using Communications.API.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Hangfire;

namespace Communications.API.Helpers
{
    public class MailHelper
    {
        #region Fields

        private MainDbContext _context;

        #endregion Fields

        #region Constructor

        public MailHelper(MainDbContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Public Methods

        [JobDisplayName("Sending Email")]
        public void ProcessMailTask(Guid id)
        {
            try
            {
                var mailTask = GetMailTask(id);
                SendEmail(mailTask);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                UpdateMailTask(id, true);
            }
        }

        public void CleanupEmails()
        {
            try
            {
                DeleteOldEmails();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void SendEmail(MailTask mailTask)
        {
            var recipientList = new InternetAddressList(mailTask.To.Split(";").Select(x => new MailboxAddress(x)));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(mailTask.From));
            message.To.AddRange(recipientList);
            message.Subject = mailTask.Subject;
            message.Body = new TextPart("html")
            {
                Text = mailTask.Body
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("sage.za.sss@gmail.com", "Sage@1234$");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        private MailTask GetMailTask(Guid id)
        {
            return _context.MailTasks.FirstOrDefault(x => x.Id == id);
        }

        private void UpdateMailTask(Guid id, bool processed)
        {
            var mailTask = _context.MailTasks.FirstOrDefault(x => x.Id == id);
            mailTask.Processed = processed;

            _context.SaveChanges();
        }

        private void DeleteOldEmails()
        {
            //var today = DateTime.Today;
            //var cutOffDate = new DateTime(today.Year, today.Month - 3, 1);

            //var oldEmails =
            //    _context.MailTasks
            //    .Where(x => x.Processed == true && x.ProcessedTimestamp.HasValue && x.ProcessedTimestamp.Value < cutOffDate);
        }

        #endregion Private Methods
    }
}
