using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.ViewModel.ViewMain
{
    public class DashboardViewModel
    {
        public List<GroupTreeNodeViewModel> GroupTree { get; set; } = new List<GroupTreeNodeViewModel>();
        public GroupUsersViewModel SelectedGroupUsers { get; set; }
    }
}