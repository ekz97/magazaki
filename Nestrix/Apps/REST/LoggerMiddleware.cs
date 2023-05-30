using System.Text;

namespace RESTLayer;

public class LoggerMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    
    public LoggerMiddleware(RequestDelegate next, ILoggerFactory logger)
    {
        _logger = logger.AddFile("Logs/Log-{Date}.txt").CreateLogger<LoggerMiddleware>();
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        // var request = await FormatRequest(context.Request);
        // _logger.LogInformation(request);
        //
        // var originalBodyStream = context.Response.Body;
        //
        // using var responseBody = new MemoryStream();
        // context.Response.Body = responseBody;
        //
        // await _next(context);
        //
        // var response = await FormatResponse(context.Response);
        // _logger.LogInformation(response);
        //
        // await responseBody.CopyToAsync(originalBodyStream);
        try
        {
            await _next(context);
        }
        finally
        {
            _logger.LogInformation("Request {method} => {statuscode}",
                context.Request?.Method,
                context.Response?.StatusCode);
        }
    }

    // private async Task<string> FormatRequest(HttpRequest request)
    // {
    //     var body = request.Body;
    //
    //     request.EnableBuffering();
    //
    //     var buffer = new byte[Convert.ToInt32(request.ContentLength)];
    //     await request.Body.ReadAsync(buffer, 0, buffer.Length);
    //     var bodyAsText = Encoding.UTF8.GetString(buffer);
    //     request.Body = body;
    //     
    //     return $"{request.Method} {request.Scheme}://{request.Host}{request.Path} {request.QueryString} {bodyAsText}";
    // }
    //
    //
    // private async Task<string> FormatResponse(HttpResponse response)
    // {
    //     response.Body.Seek(0, SeekOrigin.Begin);
    //     string text = await new StreamReader(response.Body).ReadToEndAsync();
    //     response.Body.Seek(0, SeekOrigin.Begin);
    //     
    //     return $"{response.StatusCode}: {text}";
    // }
}