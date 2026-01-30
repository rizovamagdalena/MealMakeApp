using MealMake.Domain.Domain_Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Identity
{
    public class MealMakeApplicationUser : IdentityUser 
    {
        public string? FirstName { get; set; }   
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual List<UserFavoriteMeal> FavoriteMeals { get; set; } = new();
        public virtual List<MealCollection> MealCollections { get; set; } = new();
        public virtual List<CollectionCategory> Categories { get; set; } = new();




    }
}
