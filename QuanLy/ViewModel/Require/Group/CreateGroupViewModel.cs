using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.ViewModel.Require.Group
{
    public class CreateGroupViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int OrderNumber { get; set; }
        public int? ParentGroupId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }
}