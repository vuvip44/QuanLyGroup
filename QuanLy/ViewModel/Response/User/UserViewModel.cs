using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Models.Constants;
using QuanLy.ViewModel.Response.Group;

namespace QuanLy.ViewModel.Response.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public int OrderNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public List<GroupInforViewModel> Groups { get; set; } = new List<GroupInforViewModel>();
    }
}