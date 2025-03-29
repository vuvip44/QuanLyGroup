using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.ViewModel.ViewMain
{
    public class GroupUsersViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public List<UserInGroupViewModel> Users { get; set; } = new List<UserInGroupViewModel>();
    }
}