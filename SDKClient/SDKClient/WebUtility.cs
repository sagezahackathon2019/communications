using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SDKClient
{
    internal static class WebUtility
    {
        private static Dictionary<string, HttpMethod> httpMethods = new Dictionary<string, HttpMethod>()
        {
            { "GET", HttpMethod.Get },
            { "POST", HttpMethod.Post },
            { "PUT", HttpMethod.Put },
            { "DELETE", HttpMethod.Delete },
            { "HEAD", HttpMethod.Head }
        };

        internal static string SendRequestJSON(string url, string httpMethod, string vendorKey, object postData, NameValueCollection queryValues, Tuple<string, string> credential = null)
        {
            HttpMethod method = httpMethods.Keys.Contains(httpMethod) ? httpMethods[httpMethod] : null;

            if (method == null)
            {
                throw new ArgumentException("Invalid value", nameof(httpMethod));
            }

            if (!Guid.TryParse(vendorKey, out _))
            {
                throw new ArgumentException("Invalid value", nameof(vendorKey));
            }

            if (queryValues != null)
            {
                BuildQueryUrl(url, queryValues);
            }

            JsonSerializerSettings sett = new JsonSerializerSettings();
            sett.NullValueHandling = NullValueHandling.Ignore;
            sett.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

            Uri endpoint = new Uri(url);

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(method, endpoint);
                request.Headers.Accept.Clear();
                if (!string.IsNullOrEmpty(vendorKey))
                    request.Headers.Add("X-Vendor-Key", vendorKey);

                if (postData != null)
                {
                    string body = JsonConvert.SerializeObject(postData, sett);
                    request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = httpClient.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception($"{response.StatusCode}: {response.ReasonPhrase}");
                }
            }
        }

        private static string BuildQueryUrl(string url, NameValueCollection queryValues)
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
            return url;
        }
    }
}