using MealMake.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Interface
{
    public interface IActiveCollectionService
    {
        UserActiveCollection GetActiveCollection(string userId);
        void SetActiveCollection(string userId, Guid collectionId);
        void ArchiveActiveCollection(string userId);
    }

}
