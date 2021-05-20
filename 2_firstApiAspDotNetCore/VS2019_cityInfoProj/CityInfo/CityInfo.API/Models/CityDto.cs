using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    //dto = data transfer object
    public class CityDto
    {
        //
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
