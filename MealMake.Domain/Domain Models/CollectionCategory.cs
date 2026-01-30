using MealMake.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Domain_Models
{
    public class CollectionCategory : BaseEntity
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public MealMakeApplicationUser User { get; set; }

        public virtual ICollection<MealCollectionCategory> MealCollections { get; set; }
    }
}
