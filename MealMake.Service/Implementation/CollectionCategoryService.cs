using MealMake.Domain.Domain_Models;
using MealMake.Domain.ViewModels;
using MealMake.Repository.Interface;
using MealMake.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MealMake.Service.Implementation
{
    public class CollectionCategoryService : ICollectionCategoryService
    {
        private readonly IRepository<CollectionCategory> _categoryRepository;

        public CollectionCategoryService(IRepository<CollectionCategory> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<CollectionCategory> GetAll(string userId)
        {
            return _categoryRepository
                .GetAll(selector: x => x,
                        predicate: x => x.UserId == userId)
                .ToList();
        }

        public CollectionCategory? GetById(Guid id, string? userId)
        {
            return _categoryRepository.Get(
                selector: x => x,
                predicate: x => x.Id == id && x.UserId == userId
            );
        }

        public CollectionCategory Add(CollectionCategoryCreateViewModel vm, string userId)
        {
            var category = new CollectionCategory
            {
                Id = Guid.NewGuid(),
                Name = vm.Name,
                UserId = userId
            };
            category.Id = Guid.NewGuid();
            return _categoryRepository.Insert(category);
        }

        public CollectionCategory Update(CollectionCategory category)
        {
            return _categoryRepository.Update(category);
        }

        public CollectionCategory DeleteById(Guid id, string userId)
        {
            var category = GetById(id,userId);
            if (category == null)
                throw new Exception("Category not found");

            return _categoryRepository.Delete(category);
        }
    }
}
