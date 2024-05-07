using System.Text.Json;


public class CatService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CatService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
    }

    public async Task<List<Cat>> GetAllCats()
    {
        try
        {
            var petApiUrl = _configuration["ApiSettings:PetApiUrl"];
            var catEndpoint = "/api/CatLookup/GetAllCats";

            var apiUrl = petApiUrl + catEndpoint;
            var response = await _httpClient.GetStringAsync(apiUrl);

            var catApiResponse = JsonSerializer.Deserialize<CatApiResponse>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return catApiResponse?.AllCats;
        }
        catch (HttpRequestException ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error calling API: {ex.Message}");
            return null;
        }
    }
}
