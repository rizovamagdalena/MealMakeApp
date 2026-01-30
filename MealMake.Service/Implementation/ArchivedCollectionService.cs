using MealMake.Domain.Domain_Models;
using MealMake.Repository.Interface;
using MealMake.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Implementation
{
    public class ArchivedCollectionService : IArchivedCollectionService
    {
        private readonly IRepository<ArchivedCollection> _archivedRepository;

        public ArchivedCollectionService(IRepository<ArchivedCollection> archivedRepository)
        {
            _archivedRepository = archivedRepository;
        }

        public List<ArchivedCollection> GetAllByUser(string userId)
        {
            return _archivedRepository.GetAll(
                selector: x => x,
                predicate: x => x.UserId == userId,
                include: x => x.Include(a => a.Meals)
            ).ToList();
        }

        public ArchivedCollection GetDetails(Guid id, string userId)
        {
            return _archivedRepository.Get(
                selector: x => x,
                predicate: x => x.Id == id && x.UserId == userId,
                include: x => x.Include(a => a.Meals)
            );
        }
    }
}
