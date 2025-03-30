using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating;

namespace QuanLy.ViewModel.Require
{
    public class QueryObject
    {
        public string? Name { get; set; }
        public string? Code { get; set; }

        public string? GroupName { get; set; }
        // public bool IsDecending { get; set; }

        // public string? SortBy { get; set; }
        public int Page { get; set; } =1;

        public int PageSize { get; set; } = 20;
    }
}