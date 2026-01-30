using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Domain_Models
{
    public class CollectionMeal : BaseEntity
    {
        public Guid MealCollectionId { get; set; }
        public MealCollection MealCollection { get; set; }

        public string MealId { get; set; } 
        public string MealName { get; set; }
        public string MealThumbnail { get; set; }
    }
}
