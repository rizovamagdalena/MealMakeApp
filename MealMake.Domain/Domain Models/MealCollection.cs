using MealMake.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Domain_Models
{
    public class MealCollection : BaseEntity
    {
        public string Name { get; set; }  

        public string UserId { get; set; }
        public MealMakeApplicationUser User { get; set; }

        public virtual ICollection<CollectionMeal> Meals { get; set; }
        public virtual ICollection<MealCollectionCategory> Categories { get; set; }

    }

}
