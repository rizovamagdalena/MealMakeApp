using MealMake.Domain.ViewModels;
using MealMake.Service.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MealMake.Service.Implementation
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"];
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.openai.com/v1/")
            };
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<List<IngredientSummaryViewModel>> NormalizeIngredientsAsync(
    Dictionary<string, List<string>> rawIngredients)
        {
            Console.WriteLine("Starting NormalizeIngredientsAsync...");

            var ingredientText = string.Join("\n",
                rawIngredients.Select(i =>
                    $"{i.Key}: {string.Join(", ", i.Value)}"));

            Console.WriteLine("Raw Ingredients Text:");
            Console.WriteLine(ingredientText);

            var prompt = """
                You are given a list of ingredients with measurements.
                Normalize ingredient names (case-insensitive, same meaning = same ingredient).
                Combine quantities mathematically when possible.

                Return ONLY valid JSON in this format:
                [
                   {
                     "name": "olive oil",
                     "totalAmount": "5/6 cup"
                   }
                ]

                Ingredients:
                """ + ingredientText;

            Console.WriteLine("Prompt sent to OpenAI:");
            Console.WriteLine(prompt);

            var request = new
            {
                model = "gpt-4.1-mini",
                messages = new[] { new { role = "user", content = prompt } },
                temperature = 0.1
            };

            var response = await _httpClient.PostAsJsonAsync("chat/completions", request);
            Console.WriteLine("HTTP Response Status: " + response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw JSON from OpenAI:");
            Console.WriteLine(json);

            var content = JsonDocument.Parse(json)
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            Console.WriteLine("Parsed content from OpenAI:");
            Console.WriteLine(content);

            // ====== Fix: Remove ```json or ``` if present ======
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Trim();
                if (content.StartsWith("```"))
                {
                    int startIndex = content.IndexOf('\n') + 1; // first newline after ```
                    int endIndex = content.LastIndexOf("```");
                    if (endIndex > startIndex)
                        content = content.Substring(startIndex, endIndex - startIndex).Trim();
                }
            }

            Console.WriteLine("Cleaned JSON to deserialize:");
            Console.WriteLine(content);

            var result = JsonSerializer.Deserialize<List<IngredientSummaryViewModel>>(content!);
            Console.WriteLine("Deserialized ingredient list count: " + result?.Count);

            return result ?? new List<IngredientSummaryViewModel>();
        }

    }
}
