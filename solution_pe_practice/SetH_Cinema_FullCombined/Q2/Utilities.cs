namespace Q2
{
    public class Utilities
    {
        private static string _baseUrl = null!;

        public static void Initialize(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _baseUrl = configuration["GivenAPIBaseUrl"]
                ?? throw new InvalidOperationException("GivenAPIBaseUrl is not configured in appsettings.json");
        }

        public static string GetAbsoluteUrl(string path)
        {
            if (_baseUrl == null)
                throw new InvalidOperationException("UrlUtilities has not been initialized. Call Initialize(IConfiguration) first.");

            string baseUrl = _baseUrl.TrimEnd('/');
            path = path?.TrimStart('/') ?? string.Empty;

            return $"{baseUrl}/{path}";
        }
    }
}
