using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PriorAuthorization.Manager.API.DTOs;
using PriorAuthorization.Manager.API.Services.Interfaces;

namespace PriorAuthorization.Manager.API.Services.Implementations;

public class OpenRouterService : IOpenRouterService
{
    private readonly HttpClient _httpClient;

    private readonly IConfiguration _configuration;

    private readonly ILogger<OpenRouterService> _logger;

    public OpenRouterService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<OpenRouterService> logger)
    {
        _httpClient =
            httpClientFactory.CreateClient();

        _configuration = configuration;

        _logger = logger;
    }

    public async Task<string>
        GenerateExecutiveSummaryAsync(
            string prompt)
    {
        string apiKey =
            _configuration["OpenRouter:ApiKey"]!;

        string model =
            _configuration["OpenRouter:Model"]!;

        string url =
            _configuration["OpenRouter:BaseUrl"]!;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "OpenRouter API Key is not configured.");
        }

        var requestBody = new
        {
            model,

            response_format = new
            {
                type = "json_object"
            },

            messages = new[]
            {
                new
                {
                    role = "user",
                    content = prompt
                }
            }
        };

        var request =
            new HttpRequestMessage(
                HttpMethod.Post,
                url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                apiKey);

        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue(
                "application/json"));

        request.Headers.Add(
            "HTTP-Referer",
            "http://localhost:5173");

        request.Headers.Add(
            "X-Title",
            "Prior Authorization Tracking System");

        request.Content =
            new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

        _logger.LogInformation(
            "Sending Executive Report request to OpenRouter.");

        _logger.LogInformation(
            "Prompt Length : {Length}",
            prompt.Length);

        HttpResponseMessage response =
            await _httpClient.SendAsync(
                request);

        string responseContent =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "OpenRouter Error : {Response}",
                responseContent);

            throw new Exception(
                $"OpenRouter Error : {responseContent}");
        }

        using JsonDocument document =
            JsonDocument.Parse(
                responseContent);

        string aiResponse =
     document
         .RootElement
         .GetProperty("choices")[0]
         .GetProperty("message")
         .GetProperty("content")
         .GetString()!
         .Replace("```json", "")
         .Replace("```", "")
         .Trim();

        _logger.LogInformation(
            "AI Response:\n{Response}",
            aiResponse);

        return aiResponse;
    }
}