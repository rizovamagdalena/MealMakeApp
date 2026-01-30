using MealMake.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Service.Interface
{
    public interface IArchivedCollectionService
    {
        List<ArchivedCollection> GetAllByUser(string userId);
        ArchivedCollection GetDetails(Guid id, string userId);
    }
}
