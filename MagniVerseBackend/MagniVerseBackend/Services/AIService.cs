using System.Text;
using System.Text.Json;

namespace MagniVerseBackend.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;

        public AIService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> AskAI(string prompt)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("OpenAI API key not configured.");
            }

            var requestBody = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://api.openai.com/v1/chat/completions");

            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseBody);

            var content = doc
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content ?? "";
        }
    }
}