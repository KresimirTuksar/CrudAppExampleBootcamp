using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApp.Common
{
    public class PagingModel
    {
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalResultCount { get; set; }
        public string OrderBy { get; set; } = "CreatedAt";
        public string OrderType { get; set; } = "asc";
    }
}
