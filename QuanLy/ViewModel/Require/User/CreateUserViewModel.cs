using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Models.Constants;

namespace QuanLy.ViewModel.Require.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Account is required.")]
        public string Account { get; set; }

        public int OrderNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        public List<int> GroupIds { get; set; } = new List<int>();
    }
}