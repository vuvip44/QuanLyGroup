using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Models;
using QuanLy.ViewModel.Require.User;
using QuanLy.ViewModel.Response.Group;
using QuanLy.ViewModel.Response.User;

namespace QuanLy.Mapping
{
    public static class UserMapping
    {
        public static User ToEntityFromCreateUserViewModel(this CreateUserViewModel model)
        {
            return new User
            {
                Name = model.Name,
                Email = model.Email,
                Account = model.Account,
                OrderNumber = model.OrderNumber,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                // UserGroups = model.GroupIds.Select(g => new UserGroup
                // {
                //     GroupId = g
                // }).ToList()
            };
        }

        public static User ToEntityFromUpdateUserViewModel(this UpdateUserViewModel model)
        {
            return new User
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Account = model.Account,
                OrderNumber = model.OrderNumber,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                // UserGroups = model.GroupIds.Select(g => new UserGroup
                // {
                //     GroupId = g
                // }).ToList()
            };
        }

        public static UserViewModel ToUserViewModelFromEntity(this User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Account = user.Account,
                OrderNumber = user.OrderNumber,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                Groups = user.UserGroups.Select(g => g.ToGroupInforViewModelFromUserGroup()).ToList()
            };
        }

        public static UpdateUserViewModel ToUpdateUserViewModel(this User user)
        {
            return new UpdateUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Account = user.Account,
                OrderNumber = user.OrderNumber,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                GroupIds = user.UserGroups.Select(g => g.GroupId).ToList()
            };
        }
    }
}