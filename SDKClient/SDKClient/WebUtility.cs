using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SDKClient
{
	class WebUtility
	{
		public static string SendRequestJSON(string url, string method, string vendorKey, Object PostData, NameValueCollection queryValues, Tuple<string, string> credential = null)
		{
			JsonSerializerSettings sett = new JsonSerializerSettings();
			sett.NullValueHandling = NullValueHandling.Ignore;
			sett.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

			string body = null;
			byte[] bodyBytes = null;

			if (queryValues != null)
			{
				if (queryValues.Count > 0)
				{
					url = string.Concat(url, "?");
					foreach (string name in queryValues)
					{
						url = string.Concat(url, name, "=", HttpUtility.UrlEncode(queryValues[name]), "&");
					}

					url = url.TrimEnd('&');
				}
			}

			WebClient wc = new WebClient();

			if (credential != null)
			{
				Uri uri = new Uri(url);

				var credentialCache = new CredentialCache();
				credentialCache.Add(
				  new Uri(uri.GetLeftPart(UriPartial.Authority)), // request url's host
				  "Basic",  // authentication type 
				  new NetworkCredential(credential.Item1, credential.Item2) // credentials 
				);

				wc.Credentials = credentialCache;
			}

			if (!String.IsNullOrEmpty(vendorKey))
			{
				wc.Headers.Add("X-Vendor-Key", vendorKey);
			}

			if (PostData != null)
				body = JsonConvert.SerializeObject(PostData, sett);

			if (!string.IsNullOrEmpty(body))
			{
				bodyBytes = Encoding.UTF8.GetBytes(body);
				bodyBytes = wc.UploadData(url, method, bodyBytes);
			}
			else
				bodyBytes = wc.DownloadData(url);
			return Encoding.UTF8.GetString(bodyBytes);
		}
	}
}
