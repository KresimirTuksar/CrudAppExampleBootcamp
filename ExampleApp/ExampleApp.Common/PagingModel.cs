using System;
using System.Collections.Generic;

namespace ExampleApp.Common
{
    public class PagingModel<T>
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public List<T> Results { get; set; }

    }
}