using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class ModifyDownstreamPathMiddleware
{
    private readonly RequestDelegate _next;

    public ModifyDownstreamPathMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/gateway/api/v1/customer/profile"))
        {
            // Check if the request has a body and is POST/PUT
        if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
        {
            // Enable re-reading the body
            context.Request.EnableBuffering();

            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var jsonBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

            if (jsonBody != null && jsonBody.TryGetValue("status", out var statusValue) && statusValue is bool status){
                context.Request.Body.Position = 0;

                // var status = jsonBody["status"];
                // Set header to be used for downstream path template
                // context.Items["status"] = status;
                context.Items["status"] = status ? "enableCustomer" : "disableCustomer";

                // Reset the body stream for further processing
                var bytes = Encoding.UTF8.GetBytes(body);
                context.Request.Body = new MemoryStream(bytes);
            }
        }
        }

        await _next(context);
    }
}

