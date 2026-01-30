using MealMake.Domain.Domain_Models;
using MealMake.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Interface
{
    public interface IMealService
    {
        public void ToggleFavorite(MealSave meal, string userId);
        public bool IsMealFavorite(string mealId, string userId);
        public List<UserFavoriteMeal> GetUserFavorites(string userId);
        public void RemoveFavorite(string mealId, string userId);

    }

}
