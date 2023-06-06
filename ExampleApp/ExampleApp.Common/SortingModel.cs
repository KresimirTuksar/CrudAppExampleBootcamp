using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApp.Common
{
    public class SortingModel
    {
        public string SortBy { get; set; } = "CreatedAt";
        public bool IsDescending{ get; set; }
     }
}
