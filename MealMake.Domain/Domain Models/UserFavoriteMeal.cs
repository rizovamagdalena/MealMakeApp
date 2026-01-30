using MealMake.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Domain_Models
{
    public class UserFavoriteMeal : BaseEntity
    {
        public string UserId { get; set; }
        public MealMakeApplicationUser User { get; set; }

        public string MealId { get; set; }
        public string MealName { get; set; } 

        public string MealThumbnail { get; set; }
    }
}
