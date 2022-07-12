
using ConsoleSetAvilability.Library.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleSetAvilability.Library.Logic
{
    public class WebProcessor : IWebProcessor
    {
        private ILogger<IWebProcessor> _logger;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public WebProcessor(ILogger<IWebProcessor> logger)
        {
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            _logger = logger;
        }

        public HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient(GetDefaultHandler());
            client.Timeout = TimeSpan.FromMinutes(10);
            client.DefaultRequestHeaders.Add("User-Agent", GetUserAgent());

            return client;
        }

        private HttpClientHandler GetDefaultHandler()
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler defaultHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslErrors) => true,
                UseDefaultCredentials = true,
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = cookies,
                AutomaticDecompression = DecompressionMethods.GZip,
            };

            return defaultHandler;
        }

        public string GetContent(string url, ref HttpClient client, string? encodingFormat = null)
        {
            try
            {
                if (url?.Length > 3)
                {
                    var stream = client.GetAsync(url).Result.Content.ReadAsStreamAsync().Result;

                    var encoding = GetEncodingFormat(encodingFormat);

                    StreamReader reader = new StreamReader(stream, encoding);
                    var result = reader.ReadToEnd();

                    return HttpUtility.HtmlDecode(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Nie można pobrać treści ze strony Get-method {url}" + $"{ex.Message}");
            }
            _logger.LogInformation("Nieprawidłowy link");

            return "";
        }

        public async Task<string> MakeGetRequest(string url, HttpClient client, string? encodingFormat = null)
        {
            try
            {
                if (url?.Length > 3)
                {
                    var stream = client.GetAsync(url).Result.Content.ReadAsStreamAsync().Result;

                    return await GetHtmlDecodeStream(stream, encodingFormat);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Nie można pobrać treści ze strony Get-Async method {url}" + $"{ex.Message}");
            }
            _logger.LogInformation("Nieprawidłowy link");

            return "";
        }

        public async Task<string> MakePostRequest(
            string urlPost,
            HttpContent formUrlEncoded,
            HttpClient client,
            string? encodingFormat = null)
        {
            try
            {
                if (urlPost?.Length > 3)
                {
                    var stream = client.PostAsync(urlPost, formUrlEncoded).Result.Content.ReadAsStreamAsync().Result;

                    return await GetHtmlDecodeStream(stream, encodingFormat);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Nie można pobrać treści ze strony Post-method {urlPost}" + $"{ex.Message}");
            }
            _logger.LogInformation("Nieprawidłowy link");

            return "";
        }

        public async Task<string> MakePutRequestAsync(
            string urlPost,
            HttpContent formUrlEncoded,
            HttpClient client,
            string? encodingFormat = null)
        {
            try
            {
                if (urlPost?.Length > 3)
                {
                    var stream = client.PutAsync(urlPost, formUrlEncoded).Result.Content.ReadAsStreamAsync().Result;

                    return await GetHtmlDecodeStream(stream, encodingFormat);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Bad request Put-method {urlPost}" + $"{ex.Message}");
            }
            _logger.LogInformation("Nieprawidłowy link");

            return "";
        }

        public async Task<string> ApiPost(
            string urlPost,
            HttpContent formUrlEncoded,
            HttpClient client,
            string? encodingFormat = null)
        {
            try
            {
                if (urlPost?.Length > 0)
                {
                    var encoding = GetEncodingFormat(encodingFormat);
                    var stream = client.PostAsync(urlPost, formUrlEncoded).Result.Content.ReadAsStreamAsync().Result;
                    StreamReader reader = new StreamReader(stream, encoding);

                    return await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Błąd połączenia z bazą danych Api {urlPost}" + $"{ex.Message}");
            }
            _logger.LogInformation("Nieprawidłowy link do Api");

            return "";
        }

        private async Task<string> GetHtmlDecodeStream(Stream stream, string? encodingFormat)
        {
            var encoding = GetEncodingFormat(encodingFormat);

            StreamReader reader = new StreamReader(stream, encoding);
            var result = await reader.ReadToEndAsync();

            return HttpUtility.HtmlDecode(result);
        }

        public string Login(string url, HttpContent loginParameters, ref HttpClient client)
        {
            var response = "";

            try
            {
                var res = client.PostAsync(url, loginParameters).Result.Content.ReadAsStreamAsync().Result;

                StreamReader reader = new StreamReader(res);
                response = HttpUtility.HtmlDecode(reader.ReadToEnd());

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Nie można zlogować do strony {url}" + ex.Message);
            }

            return response;
        }

        public Encoding GetEncodingFormat(string? encodingFormat)
        {
            Encoding.RegisterProvider(provider: CodePagesEncodingProvider.Instance);
            var isSetEncodingFormat = encodingFormat == null ? false : true;

            if (isSetEncodingFormat)
            {
                return Encoding.GetEncoding(encodingFormat);
            }

            return Encoding.Default;
        }

        private string GetUserAgent()
        {
            return "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:81.0) Gecko/20100101 Firefox/81.0";
        }

        public string LoginToApi(string url, ref HttpClient client)
        {
            var loginParameters = new Dictionary<string, string>() 
            {
                    { "email", "synchronizator@isprzet.pl" },
                    { "password", "Hakunamatata2020@!" }
            };

            var jsonData = JsonConvert.SerializeObject(loginParameters, _jsonSerializerSettings);
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, httpContent).Result.Content.ReadAsStreamAsync().Result;
            StreamReader reader = new StreamReader(response);

            return HttpUtility.HtmlDecode(reader.ReadToEnd()); ;
        }
    }
}
