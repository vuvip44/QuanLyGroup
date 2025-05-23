using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.ViewModel.ViewMain
{
    public class GroupTreeNodeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text => Name;
        public int? ParentId { get; set; }
        public List<GroupTreeNodeViewModel> Children { get; set; } = new List<GroupTreeNodeViewModel>();
        public string Code { get; set; }
        public string ICon { get; set; }
        public int OrderNumber { get; set; }
    }
}