using MealMake.Domain.Domain_Models;
using MealMake.Domain.Dto;
using MealMake.Domain.ViewModels;
using MealMake.Repository.Interface;
using MealMake.Service.Interface;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MealMake.Service.Implementation
{
    public class MealService : IMealService
    {
        private readonly IRepository<UserFavoriteMeal> _userFavoriteMealRepository;


        public MealService(IRepository<UserFavoriteMeal> userFavoriteMealRepository)
        {
            _userFavoriteMealRepository = userFavoriteMealRepository;
        }

        public void ToggleFavorite(MealSave meal, string userId)
         {
            Debug.WriteLine($"[MarkMealAsFavoriteAsync] User: {userId}, Meal: {meal.mealId} - {meal.mealName}");



            var favorite = _userFavoriteMealRepository.Get(
               selector: f => f,
               predicate: f => f.UserId == userId && f.MealId == meal.mealId
           );

            if (favorite == null)
            {
                Debug.WriteLine("[MarkMealAsFavoriteAsync] Favorite not found, adding favorite.");

                _userFavoriteMealRepository.Insert(new UserFavoriteMeal
                {
                    UserId = userId,
                    MealId = meal.mealId,
                    MealName = meal.mealName,
                    MealThumbnail = meal.mealThumbnail
                });

                Debug.WriteLine("[MarkMealAsFavoriteAsync] Favorite successfully added.");
            }
            else
            {
                _userFavoriteMealRepository.Delete(favorite);
               Debug.WriteLine("[MarkMealAsFavoriteAsync] Favorite successfully removed.");
            }
        }


        public bool IsMealFavorite(string mealId, string userId)
        {
            var favorite = _userFavoriteMealRepository.Get(
                selector: f => f,
                predicate: f => f.UserId == userId && f.MealId == mealId
            );

            return favorite != null;
        }

        public List<UserFavoriteMeal> GetUserFavorites(string userId)
        {
            return _userFavoriteMealRepository.GetAll(
                selector: f => f,
                predicate: f => f.UserId == userId
            ).ToList();
        }

        public void RemoveFavorite(string mealId, string userId)
        {
            var favorite = _userFavoriteMealRepository.Get(
                selector: f => f,
                predicate: f => f.UserId == userId && f.MealId.ToString() == mealId
            );

            if (favorite != null)
            {
                _userFavoriteMealRepository.Delete(favorite);
                Debug.WriteLine("[RemoveFavorite] Favorite removed.");
            }
        }

    }
}
