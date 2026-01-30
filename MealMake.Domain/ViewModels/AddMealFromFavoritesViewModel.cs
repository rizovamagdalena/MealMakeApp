using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.ViewModels
{
    public class AddMealFromFavoritesViewModel
    {
        public Guid CollectionId { get; set; }
        public string CollectionName { get; set; }
        public List<MealViewModel> FavoriteMeals { get; set; } = new();
    }
}
