using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace JumpServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JumpController : ControllerBase
    {

        private readonly ILogger<JumpController> _logger;

        public JumpController(ILogger<JumpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public async Task<string> Go()
        {
            var req = Request;
            if (!req.Headers.TryGetValue("url", out var url))
            {
                return "must provide url in header";
            }
            var client = new HttpClient { };
            HashSet<string>? headerSets = null;
            if (req.Headers.TryGetValue("accepted-headers", out var accecptedHeaders))
            {
                headerSets = accecptedHeaders.ToString().Split(',').ToHashSet();
            }
            req.EnableBuffering();
            using var bodyReader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true);
            var body = await bodyReader.ReadToEndAsync();
            if (req.Headers.TryGetValue("Content-Type", out var contentType))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            }

            var content = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = new HttpMethod(Request.Method),
                Content = new StringContent(body, Encoding.UTF8, contentType),
            };
            if (headerSets != null)
            {
                foreach (var header in req.Headers)
                {
                    if (headerSets.Contains(header.Key))
                    {
                        content.Headers.Add(header.Key, header.Value.ToString());
                    }
                }
            }
            var response = await client.SendAsync(content);

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}