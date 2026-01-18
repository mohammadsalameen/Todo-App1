using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.Common.Pagination
{
    public class Pagination
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public string Status { get; set; } = "all";
    }
}
