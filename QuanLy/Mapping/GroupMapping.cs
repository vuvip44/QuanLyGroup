using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using QuanLy.Models;
using QuanLy.ViewModel.Require.Group;
using QuanLy.ViewModel.Response.Group;
using QuanLy.ViewModel.ViewMain;

namespace QuanLy.Mapping
{
    public static class GroupMapping
    {
        public static Group ToEntityFromCreateGroup(this CreateGroupViewModel model)
        {
            return new Group()
            {
                Name = model.Name,
                Code = model.Code,
                OrderNumber = model.OrderNumber,
                ParentGroupId = model.ParentGroupId,
                UserGroups = model.UserIds.Select(u => new UserGroup()
                {
                    UserId = u
                }).ToList()
            };
        }

        public static Group ToEntityFromUpdateGroup(this UpdateGroupViewModel model)
        {
            return new Group()
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                OrderNumber = model.OrderNumber,
                ParentGroupId = model.ParentGroupId,

            };
        }

        public static GroupViewModel ToGroupViewModelFromEntity(this Group group)
        {
            return new GroupViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                Code = group.Code,
                OrderNumber = group.OrderNumber,
                ParentGroupId = group.ParentGroupId,

            };
        }
    }
}