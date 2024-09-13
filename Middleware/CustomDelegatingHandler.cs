public class CustomDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomDelegatingHandler> _logger;

    public CustomDelegatingHandler(IHttpContextAccessor httpContextAccessor, ILogger<CustomDelegatingHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        _logger.LogDebug("CustomDelegatingHandler invoked with request URI: {Uri}", request.RequestUri);

        if (context!.Items.ContainsKey("status"))
        {
          var extractedValue = context.Items["status"]!.ToString();
          _logger.LogDebug("Extracted value from request body: {Value}", extractedValue);

          // Decode the downstream URL
          var decodedPath = System.Net.WebUtility.UrlDecode(request.RequestUri!.AbsolutePath);

          // Modify the downstream UR
          var newPath = decodedPath.Replace("{status}", extractedValue);
          _logger.LogDebug("New downstream path: {NewPath}", newPath);

          var uriBuilder = new UriBuilder(request.RequestUri)
          {
              Path = newPath
          };
          request.RequestUri = uriBuilder.Uri;
        }
        else
        {
            _logger.LogWarning("Extracted value not found in the context.");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
