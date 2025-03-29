using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.ViewModel.Response
{
    public class PageResponse<T> where T : class
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPage { get; set; }

        public long TotalElement { get; set; }

        public dynamic Data { get; set; }
    }
}