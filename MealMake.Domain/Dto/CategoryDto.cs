using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Dto
{
    public class CategoryDto
    {
        public string StrCategory { get; set; }
    }

    public class CategoryApiResponse
    {
        public List<CategoryDto> Meals { get; set; } = new();
    }

}
