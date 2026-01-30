using MealMake.Domain.Domain_Models;
using MealMake.Repository.Interface;
using MealMake.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Implementation
{
    public class ActiveCollectionService : IActiveCollectionService
    {
        private readonly IRepository<UserActiveCollection> _activeCollectionRepository;
        private readonly IRepository<ArchivedCollection> _archivedRepository;
        private readonly IMealCollectionService _mealCollectionService;

        public ActiveCollectionService(
            IRepository<UserActiveCollection> activeCollectionRepository,
            IRepository<ArchivedCollection> archiveRepository,
            IMealCollectionService mealCollectionService)
        {
            _activeCollectionRepository = activeCollectionRepository;
            _archivedRepository = archiveRepository;
            _mealCollectionService = mealCollectionService;
        }

        public UserActiveCollection GetActiveCollection(string userId)
        {
            return _activeCollectionRepository.Get(selector: x => x,
                                                    predicate: x => x.UserId == userId);
        }

        public void SetActiveCollection(string userId, Guid collectionId)
        {
            var old = _activeCollectionRepository.Get(selector: x => x,
                                                    predicate: x => x.UserId == userId);
            if (old != null) _activeCollectionRepository.Delete(old);

            _activeCollectionRepository.Insert(new UserActiveCollection
            {
                UserId = userId,
                CollectionId = collectionId
            });
        }

        public void ArchiveActiveCollection(string userId)
        {
            var active = GetActiveCollection(userId);
            if (active == null) return;

            var collection = _mealCollectionService.GetById(active.CollectionId, userId);
            if (collection == null) return;

            var archived = new ArchivedCollection
            {
                UserId = userId,
                CollectionName = collection.Name,
                Meals = collection.Meals.Select(m => new ArchivedCollectionMeal
                {
                    MealId = m.MealId,
                    MealName = m.MealName,
                    MealThumbnail = m.MealThumbnail
                }).ToList()
            };

            _archivedRepository.Insert(archived);

            _mealCollectionService.DeleteById(collection.Id, userId);

            var activeToDelete = _activeCollectionRepository.Get(selector: x => x,
                                                         predicate: x => x.UserId == userId);
            if (activeToDelete != null)
                _activeCollectionRepository.Delete(activeToDelete);

        }
    }

}
