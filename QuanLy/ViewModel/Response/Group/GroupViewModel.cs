using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.ViewModel.Response.Group
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int OrderNumber { get; set; }
        public int? ParentGroupId { get; set; }
        public string? ParentGroupName { get; set; }

    }
}