using MealMake.Domain.Domain_Models;
using MealMake.Domain.Dto;
using MealMake.Domain.ViewModels;
using MealMake.Service.Interface;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace MealMake.Service.Implementation
{
    public class DataFetchService : IDataFetchService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IMealService _mealService;

        public DataFetchService(HttpClient httpClient, IMealService mealService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _mealService = mealService;
        }

        //private async Task<MealViewModel> MapToViewModelAsync(MealDto dto, string userId)
        //{
        //    if (dto == null) return null;

        //    return new MealViewModel
        //    {
        //        Id = dto.IdMeal,
        //        Name = dto.StrMeal,
        //        Category = dto.StrCategory,
        //        Area = dto.StrArea,
        //        Instructions = dto.StrInstructions,
        //        Thumbnail = dto.StrMealThumb,
        //        YoutubeUrl = dto.StrYoutube,
        //        Ingredients = dto.Ingredients?.Where(i => !string.IsNullOrWhiteSpace(i.Ingredient))
        //                                      .Select(i => new IngredientViewModel
        //                                      {
        //                                          Name = i.Ingredient,
        //                                          Measure = i.Measure
        //                                      }).ToList() ?? new List<IngredientViewModel>(),
        //        IsFavorite = userId != null ? _mealService.IsMealFavorite(dto.IdMeal, userId) : false
        //    };
        //}

        private async Task<MealViewModel> MapToViewModelAsync(MealDto dto, string userId)
        {
            if (dto == null) return null;

            // Collect ingredients from strIngredient1..20
            var ingredients = new List<IngredientViewModel>();
            for (int i = 1; i <= 20; i++)
            {
                Console.WriteLine($"Checking ingredient {i}");

                var ingredientProperty = dto.GetType().GetProperty($"StrIngredient{i}");
                var measureProperty = dto.GetType().GetProperty($"StrMeasure{i}");
                Console.WriteLine($"Ingredient prop exists: {ingredientProperty != null}");
                Console.WriteLine($"Measure prop exists: {measureProperty != null}");
                if (ingredientProperty == null || measureProperty == null) continue;

                var ingredient = ingredientProperty.GetValue(dto)?.ToString();
                var measure = measureProperty.GetValue(dto)?.ToString();

                if (!string.IsNullOrWhiteSpace(ingredient))
                {
                    ingredients.Add(new IngredientViewModel
                    {
                        Name = ingredient.Trim(),
                        Measure = measure?.Trim() ?? ""
                    });
                }
            }

            return new MealViewModel
            {
                Id = dto.IdMeal,
                Name = dto.StrMeal,
                Category = dto.StrCategory,
                Area = dto.StrArea,
                Instructions = dto.StrInstructions,
                Thumbnail = dto.StrMealThumb,
                YoutubeUrl = dto.StrYoutube,
                Ingredients = ingredients,
                IsFavorite = userId != null ? _mealService.IsMealFavorite(dto.IdMeal, userId) : false
            };
        }

        private async Task<List<MealViewModel>> MapToViewModelsAsync(List<MealDto> dtos, string userId)
        {
            if (dtos == null || !dtos.Any()) return new List<MealViewModel>();

            var list = new List<MealViewModel>();
            foreach (var dto in dtos)
                list.Add(await MapToViewModelAsync(dto, userId));
            return list;
        }

        public async Task<List<MealViewModel>> SearchMealsByNameAsync(string name, string userId)
        {
            var mealsDto = await FetchFromApiAsync<MealApiResponse>($"search.php?s={name}");
            return await MapToViewModelsAsync(mealsDto?.Meals, userId);
        }

        public async Task<List<MealViewModel>> ListAllMealsByFirstLetterAsync(char letter, string userId)
        {
            var mealsDto = await FetchFromApiAsync<MealApiResponse>($"search.php?f={letter}");
            return await MapToViewModelsAsync(mealsDto?.Meals, userId);
        }

        public async Task<List<MealViewModel>> ListAllMealsByCategoryAsync(string category, string userId)
        {
            var mealsDto = await FetchFromApiAsync<MealApiResponse>($"filter.php?c={category}");
            return await MapToViewModelsAsync(mealsDto?.Meals, userId);
        }

        public async Task<List<MealViewModel>> ListAllMealsByAreaAsync(string area, string userId)
        {
            var mealsDto = await FetchFromApiAsync<MealApiResponse>($"filter.php?a={area}");
            return await MapToViewModelsAsync(mealsDto?.Meals, userId);
        }

        public async Task<List<MealViewModel>> ListAllMealsByIngredientAsync(string ingredient, string userId)
        {
            var mealsDto = await FetchFromApiAsync<MealApiResponse>($"filter.php?i={ingredient}");
            return await MapToViewModelsAsync(mealsDto?.Meals, userId);
        }

        public async Task<MealViewModel> GetMealDetailsAsync(string id, string userId)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var response = await _httpClient.GetAsync($"lookup.php?i={id}");
            if (!response.IsSuccessStatusCode) return null;

            var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer.DeserializeAsync<MealApiResponse>(responseStream, _options);
            var mealDto = apiResponse?.Meals?.FirstOrDefault();
            return await MapToViewModelAsync(mealDto, userId);
        }

       

        public async Task<List<string>> ListMealCategoriesAsync()
        {
            var result = await FetchFromApiAsync<CategoryApiResponse>("list.php?c=list");
            return result?.Meals?.Select(c => c.StrCategory).ToList() ?? new List<string>();
        }

        public async Task<List<string>> ListAreasAsync()
        {
            var result = await FetchFromApiAsync<AreaApiResponse>("list.php?a=list");
            return result?.Meals?.Select(a => a.StrArea).ToList() ?? new List<string>();
        }

        private async Task<T?> FetchFromApiAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<T>(response, _options);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FetchFromApiAsync] Error fetching {url}: {ex.Message}");
                return default;
            }
        }

    }
}
