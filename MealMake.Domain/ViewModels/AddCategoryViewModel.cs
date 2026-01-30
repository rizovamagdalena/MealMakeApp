using MealMake.Domain.Domain_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace MealMake.Domain.ViewModels
{
    public class AddCategoryViewModel
    {
        public Guid CollectionId { get; set; }
        public string CollectionName { get; set; }
        public List<CollectionCategory> Categories { get; set; } = new List<CollectionCategory>();
        public Guid SelectedCategoryId { get; set; }
    }
}
