using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Models.Constants;

namespace QuanLy.ViewModel.Require.User
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public int OrderNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public List<int> GroupIds { get; set; } = new List<int>();
    }
}