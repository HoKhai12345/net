using Newtonsoft.Json;

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
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        context.Response.Body = originalBodyStream;
        responseBody.Seek(0, SeekOrigin.Begin);
        var bodyText = await new StreamReader(responseBody).ReadToEndAsync();

        var apiResponse = new
        {
            status = context.Response.StatusCode,
            message = (context.Response.StatusCode == 200 || context.Response.StatusCode == 201) ? "Success" : "Error",
            data = string.IsNullOrWhiteSpace(bodyText) ? null : JsonConvert.DeserializeObject(bodyText)
        };

        var json = JsonConvert.SerializeObject(apiResponse);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}