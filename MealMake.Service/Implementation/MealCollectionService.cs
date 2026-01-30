using MealMake.Domain.Domain_Models;
using MealMake.Domain.Dto;
using MealMake.Domain.ViewModels;
using MealMake.Repository.Interface;
using MealMake.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Implementation
{
    public class MealCollectionService : IMealCollectionService
    {
        private readonly IRepository<MealCollection> _mealCollectionRepository;
        private readonly IRepository<CollectionMeal> _collectionMealRepository;
        private readonly IRepository<MealCollectionCategory> _collectionCategoryRepository;
        private readonly IDataFetchService _dataFetchService;
        private readonly IRepository<UserFavoriteMeal> _userFavoriteMealRepository;
        private readonly IOpenAIService _openAIService;


        public MealCollectionService(
            IRepository<MealCollection> mealCollectionRepository,
            IRepository<CollectionMeal> collectionMealRepository,
            IRepository<MealCollectionCategory> collectionCategoryRepository,
            IRepository<UserFavoriteMeal> userFavoriteMealRepository,
            IDataFetchService dataFetchService,
            IOpenAIService openAIService    )
        {
            _mealCollectionRepository = mealCollectionRepository;
            _collectionMealRepository = collectionMealRepository;
            _collectionCategoryRepository = collectionCategoryRepository;
            _userFavoriteMealRepository = userFavoriteMealRepository;
            _dataFetchService = dataFetchService;
            _openAIService = openAIService;
        }

        public MealCollection Add(MealCollectionCreateViewModel vm, string userId)
        {
            var collection = new MealCollection
            {
                Id = Guid.NewGuid(),
                Name = vm.Name,
                UserId = userId
            };

            return _mealCollectionRepository.Insert(collection);
        }

        public MealCollection? DeleteById(Guid id, string userId)
        {
            var collection = _mealCollectionRepository.Get(
                selector: x => x,
                predicate: x => x.Id == id && x.UserId == userId
            );

            if (collection == null) return null;

            return _mealCollectionRepository.Delete(collection);
        }

        public List<MealCollection> GetAll(string userId)
        {
            return _mealCollectionRepository
                .GetAll(
                    selector: x => x,
                    predicate: x => x.UserId == userId,
                    include: q => q
                        .Include(c => c.Meals)
                        .Include(c => c.Categories)
                            .ThenInclude(cc => cc.CollectionCategory)
                )
                .ToList();
        }


        public MealCollection? GetById(Guid id, string userId)
        {
            return _mealCollectionRepository.Get(
                selector: x => x,
                predicate: x => x.Id == id && x.UserId == userId,
                include: q => q
                    .Include(c => c.Meals)
                    .Include(c => c.Categories)
                        .ThenInclude(cc => cc.CollectionCategory)
            );
        }


        public MealCollection Update(MealCollection mealCollection, string userId)
        {
            var existing = _mealCollectionRepository.Get(
                selector: x => x,
                predicate: x => x.Id == mealCollection.Id && x.UserId == userId
            );

            if (existing == null)
                throw new Exception("Collection not found or you don't have access.");

            existing.Name = mealCollection.Name;

            return _mealCollectionRepository.Update(existing);
        }

     

        public void AddCategoryToCollection(Guid collectionId, Guid categoryId)
        {
            var collection = _mealCollectionRepository.Get(
                selector: x => x,
                predicate: x => x.Id == collectionId
            );

            if (collection == null)
                throw new Exception("Meal collection not found");

            var alreadyExists = _collectionCategoryRepository
                .GetAll(
                    selector: x => x,
                    predicate: x => x.MealCollectionId == collectionId
                                  && x.CollectionCategoryId == categoryId
                )
                .Any();

            if (alreadyExists)
                return;

            var collectionCategory = new MealCollectionCategory
            {
                Id = Guid.NewGuid(),
                MealCollectionId = collectionId,
                CollectionCategoryId = categoryId
            };

            _collectionCategoryRepository.Insert(collectionCategory);
        }

        public void RemoveCategoryFromCollection(Guid collectionCategoryId)
        {
            var categoryLink = _collectionCategoryRepository.Get(
                selector: x => x,
                predicate: x => x.Id == collectionCategoryId
            );

            if (categoryLink == null)
                return;

            _collectionCategoryRepository.Delete(categoryLink);
        }

        public void AddMealToCollection(Guid collectionId, AddMealDto meal, string userId)

        {
            if (meal == null || string.IsNullOrEmpty(meal.IdMeal))
                throw new ArgumentException("Invalid meal data");

            var collection = _mealCollectionRepository.Get(
                selector: x => x,
                predicate: x => x.Id == collectionId && x.UserId == userId

            );

            if (collection == null)
                throw new Exception("Meal collection not found");

            var alreadyExists = _collectionMealRepository
                .GetAll(
                    selector: x => x,
                    predicate: x => x.MealCollectionId == collectionId
                                  && x.MealId == meal.IdMeal
                )
                .Any();

            if (alreadyExists)
                return;

            var collectionMeal = new CollectionMeal
            {
                Id = Guid.NewGuid(),
                MealCollectionId = collectionId,
                MealId = meal.IdMeal,
                MealName = meal.Name,
                MealThumbnail = meal.Thumbnail
            };

            _collectionMealRepository.Insert(collectionMeal);
        }

        public void RemoveMealFromCollection(Guid collectionId, string mealId, string userId)
        {
            var collection = _mealCollectionRepository.Get(
                selector: x => x,
                predicate: x => x.Id == collectionId && x.UserId == userId
            );

            if (collection == null)
                throw new Exception("Meal collection not found");

            var meal = _collectionMealRepository.Get(
                selector: x => x,
                predicate: x => x.MealCollectionId == collectionId && x.MealId == mealId
            );

            if (meal == null)
                return;

            _collectionMealRepository.Delete(meal);
        }

        public bool IsMealInCollection(Guid collectionId, string mealId)
        {
            var meal = _collectionMealRepository.Get(
                selector: x => x,
                predicate: x => x.MealCollectionId == collectionId && x.MealId == mealId
            );

            return meal != null;
        }

   
        public Dictionary<string, Guid> GetCollectionMealMap(Guid collectionId)
        {
            return _collectionMealRepository
                .GetAll(
                    selector: cm => new { cm.MealId, cm.Id },
                    predicate: cm => cm.MealCollectionId == collectionId
                )
                .ToDictionary(x => x.MealId, x => x.Id);
        }

        public async Task<List<IngredientSummaryViewModel>> GetIngredientSummaryForCollectionAsync(
      MealCollection collection, string userId)
        {
            if (collection == null || collection.Meals == null || !collection.Meals.Any())
                return new List<IngredientSummaryViewModel>();

            var summary = new Dictionary<string, List<string>>();

            foreach (var meal in collection.Meals)
            {
                var mealDetails = await _dataFetchService.GetMealDetailsAsync(meal.MealId, userId);
                if (mealDetails?.Ingredients == null) continue;

                foreach (var ing in mealDetails.Ingredients)
                {
                    if (string.IsNullOrWhiteSpace(ing.Name)) continue;

                    var key = ing.Name.Trim();
                    var measure = ing.Measure?.Trim() ?? "";

                    if (!summary.ContainsKey(key))
                        summary[key] = new List<string>();

                    summary[key].Add(measure);
                }
            }

            // 🔥 AI normalization step
            var normalizedIngredients = await _openAIService.NormalizeIngredientsAsync(summary);
            return normalizedIngredients;
        }

    }

}
