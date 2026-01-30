using MealMake.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Domain_Models
{
    public class ArchivedCollection : BaseEntity
    {
        public string UserId { get; set; }
        public MealMakeApplicationUser User { get; set; }

        public string CollectionName { get; set; }

        public virtual ICollection<ArchivedCollectionMeal> Meals { get; set; } = new List<ArchivedCollectionMeal>();

    }
    public class ArchivedCollectionMeal : BaseEntity
    {
        public Guid ArchivedCollectionId { get; set; }
        public ArchivedCollection ArchivedCollection { get; set; }

        public string MealId { get; set; }
        public string MealName { get; set; }
        public string MealThumbnail { get; set; }
    }
}
