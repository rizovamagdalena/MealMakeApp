using MealMake.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Interface
{
    public interface IOpenAIService
    {
        Task<List<IngredientSummaryViewModel>> NormalizeIngredientsAsync(
            Dictionary<string, List<string>> rawIngredients);
    }

}
