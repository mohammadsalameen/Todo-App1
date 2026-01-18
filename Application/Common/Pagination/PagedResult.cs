using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.Common.Pagination
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

}
