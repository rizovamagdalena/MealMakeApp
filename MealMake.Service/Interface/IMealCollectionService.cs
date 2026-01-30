using MealMake.Domain.Domain_Models;
using MealMake.Domain.Dto;
using MealMake.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Interface
{
    public interface IMealCollectionService
    {
        List<MealCollection> GetAll(string userId);
        MealCollection? GetById(Guid Id, string userId);
        MealCollection Update(MealCollection product, string userId);
        MealCollection DeleteById(Guid Id, string userId);
        MealCollection Add(MealCollectionCreateViewModel product, string userId);

        void AddMealToCollection(Guid collectionId, AddMealDto meal, string userId);
        public void RemoveMealFromCollection(Guid collectionId, string mealId, string userId);

        void AddCategoryToCollection(Guid collectionId, Guid categoryId);
        void RemoveCategoryFromCollection(Guid collectionCategoryId);
        public bool IsMealInCollection(Guid collectionId, string mealId);
        public Dictionary<string, Guid> GetCollectionMealMap(Guid collectionId);
        public Task<List<IngredientSummaryViewModel>> GetIngredientSummaryForCollectionAsync(MealCollection collection, string userId);
    }
}
