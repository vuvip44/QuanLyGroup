using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Models.Constants;

namespace QuanLy.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public int OrderNumber { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}