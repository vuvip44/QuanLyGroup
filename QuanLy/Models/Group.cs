using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.Models
{
    [Table("Groups")]
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int OrderNumber { get; set; }

        public int? ParentGroupId { get; set; }

        public virtual Group? ParentGroup { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}