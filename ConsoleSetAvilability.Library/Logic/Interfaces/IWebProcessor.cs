
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Logic.Interfaces
{
    public interface IWebProcessor
    {
        public string Login(string url, HttpContent loginParameters, ref HttpClient client);
        string GetContent(string url, ref HttpClient client, string? encodingFormat = null);

        Task<string> MakeGetRequest
            (string Url,
            HttpClient client,
            string? encodingFormat = null);

        Task<string> MakePostRequest
            (string link,
            HttpContent formUrlEncoded,
            HttpClient client,
            string? encodingFormat = null);
        Task<string> ApiPost(
            string urlPost,
            HttpContent formUrlEncoded,
            HttpClient client,
            string? encodingFormat = null);

        HttpClient GetHttpClient();
        string LoginToApi(string loginUrl, ref HttpClient clientApi);

        Task<string> MakePutRequestAsync(
            string urlPost,
            HttpContent formUrlEncoded,
            HttpClient client,
            string? encodingFormat = null);
    }
}