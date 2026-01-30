using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealMake.Domain.Dto
{
    public class AreaDto
    {
        public string StrArea { get; set; } = string.Empty;
    }
    public class AreaApiResponse
    {
        public List<AreaDto> Meals { get; set; } = new();
    }
}
