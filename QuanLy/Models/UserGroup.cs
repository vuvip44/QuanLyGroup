using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.Models
{
    [Table("UserGroups")]
    public class UserGroup
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}