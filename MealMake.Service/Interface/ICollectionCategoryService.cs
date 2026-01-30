using MealMake.Domain.Domain_Models;
using MealMake.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Interface
{
    public interface ICollectionCategoryService
    {
        List<CollectionCategory> GetAll(string userId);
        CollectionCategory? GetById(Guid id, string userId);
        CollectionCategory Add(CollectionCategoryCreateViewModel category, string userId);
        CollectionCategory Update(CollectionCategory category);
        CollectionCategory DeleteById(Guid id, string userId);
    }
}
