using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Solstice.CodingChallenge.API.Dtos.Responses;
using Solstice.CodingChallenge.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Controllers.Middleware
{
    public class IOMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<IOMiddleware> _logger;

        public IOMiddleware(RequestDelegate next, ILogger<IOMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Task<IFormCollection> url1 = context.Request.ReadFormAsync();
            Stream originalRequestBody = context.Request.Body;
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await LogException(ex, url1, context);
            }
        }

        private async Task<string> ReadRequest(HttpRequest request)
        {
            using (var bodyReader = new StreamReader(request.Body))
            {
                string body = await bodyReader.ReadToEndAsync();

                request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));

                return body;
            }
        }


        private Task LogException(Exception ex, Task<IFormCollection> body, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            var result = JsonConvert.SerializeObject(new ErrorResponseDto() { Message = "A server error has ocurred", StackTrace = ex.StackTrace });
            var log = JsonConvert.SerializeObject(new ErrorLog() { Message = "A server error has ocurred", StackTrace = ex.StackTrace });
            _logger.LogError(500, log);
            return context.Response.WriteAsync(result);
        }

    }
}
