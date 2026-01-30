using MealMake.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Domain_Models
{
    public class UserActiveCollection : BaseEntity
    {
        public string UserId { get; set; }
        public MealMakeApplicationUser User { get; set; }

        public Guid CollectionId { get; set; }
        public MealCollection Collection { get; set; }

    }
}
