using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.ViewModels
{
    public class MealViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public string Instructions { get; set; }
        public string Thumbnail { get; set; }
        public string YoutubeUrl { get; set; }
        public List<IngredientViewModel> Ingredients { get; set; } = new();
        public bool IsFavorite { get; set; }

        public bool IsInCollection { get; set; }
        public Guid? CollectionMealId { get; set; }
    }

    public class IngredientViewModel
    {
        public string Name { get; set; }
        public string Measure { get; set; }
    }
}



