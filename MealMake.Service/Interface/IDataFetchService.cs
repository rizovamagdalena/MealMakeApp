using MealMake.Domain.Domain_Models;
using MealMake.Domain.Dto;
using MealMake.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealMake.Service.Interface
{
    public interface IDataFetchService
    {
        Task<List<MealViewModel>> SearchMealsByNameAsync(string name, string userId);
        Task<List<MealViewModel>> ListAllMealsByFirstLetterAsync(char letter, string userId);
        Task<List<MealViewModel>> ListAllMealsByCategoryAsync(string category, string userId);
        Task<List<MealViewModel>> ListAllMealsByAreaAsync(string area, string userId);
        Task<List<MealViewModel>> ListAllMealsByIngredientAsync(string ingredient, string userId);
        Task<MealViewModel> GetMealDetailsAsync(string id, string userId);
        Task<List<string>> ListMealCategoriesAsync();
        Task<List<string>> ListAreasAsync();


    }
}
