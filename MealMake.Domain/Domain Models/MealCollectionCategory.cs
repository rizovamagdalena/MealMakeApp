using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Domain_Models
{
    public class MealCollectionCategory : BaseEntity
    {
        public Guid MealCollectionId { get; set; }
        public MealCollection MealCollection { get; set; }

        public Guid CollectionCategoryId { get; set; }
        public CollectionCategory CollectionCategory { get; set; }
    }
}
