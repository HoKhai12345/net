using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TransportApi.Middleware
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                // Only wrap successful JSON responses
                var isSuccess = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;
                responseBody.Seek(0, SeekOrigin.Begin);
                var bodyText = await new StreamReader(responseBody).ReadToEndAsync();

                // Reset to original stream before writing
                context.Response.Body = originalBodyStream;

                var contentType = context.Response.ContentType ?? string.Empty;
                var isJson = contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(contentType);

                if (isSuccess && isJson)
                {
                    object? data = null;
                    if (!string.IsNullOrWhiteSpace(bodyText))
                    {
                        try { data = JsonConvert.DeserializeObject(bodyText); } catch { data = bodyText; }
                    }

                    var apiResponse = new
                    {
                        status = context.Response.StatusCode,
                        message = (context.Response.StatusCode == 200 || context.Response.StatusCode == 201) ? "Success" : "Success",
                        data
                    };

                    var json = JsonConvert.SerializeObject(apiResponse);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    // Pass-through for non-success or non-JSON
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch
            {
                // Restore original body and rethrow so developer error page or exception handler can render
                context.Response.Body = originalBodyStream;
                throw;
            }
            finally
            {
                responseBody.Dispose();
            }
        }
    }
}