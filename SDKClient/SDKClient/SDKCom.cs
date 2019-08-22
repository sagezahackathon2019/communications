using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;


namespace SDKClient
{
	public class SDKComs
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string VendoKey { get; set; }
		public string ClietID { get; set; }
		public string ComsURL { get; set; }
		public string VendorKey { get; set; }
		public string returnValue { get; set; }

		public void SendEmailTask ()
		{
			try
			{

				string result = "";
				string body = null;
				//Which one to use????
				MailObject mailObject = new MailObject
				{
					Body = Body,
					ClientId = ClietID,
					From = From,
					Subject = Subject,
					To = To.Split(';')
				};

				result = WebUtility.SendRequestJSON($"{ComsURL}/api/communications/mail/submit", "POST", VendorKey, mailObject, null);

				
				returnValue = result;
				return;
			}
			catch (Exception exc)
			{
				returnValue = $"ERROR : {exc.Message}";
			}

			return;
		}

		public void GetEmailTaskStatus(string id)
		{
			try
			{
				string result = "";
				result = WebUtility.SendRequestJSON($"{ComsURL}/api/communications/mail/status/{id}", "GET", VendorKey, null, null);

				returnValue = result;
				return;
			}
			catch (Exception exc)
			{
				returnValue = $"ERROR : {exc.Message}";
			}

			return;
		}

		public void GetReportByClientID()
		{
			try
			{
				string result = "";
				result = WebUtility.SendRequestJSON($"{ComsURL}/api/communications/mail/status/all?vendorId={VendorKey}", "GET", VendorKey, null, null);
				//string result = "";
				//using (var client = new WebClient())
				//{
				//	client.Headers.Add("X-Vendor-Key", VendorKey);
				//	result = client.UploadString($"{ComsURL}/api/communications/mail/status/all?vendorId={VendorKey}& clientId={ClietID}", "GET", "");
				//}

				returnValue = result;
				return;
			}
			catch (Exception exc)
			{
				returnValue = $"ERROR : {exc.Message}";
			}

			return;
		}
	}
}
