using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.ViewModel.Require.User;

namespace QuanLy.Infrastructure.Service.IService
{
    public interface IUserGroupService
    {
        Task<bool> AddUserToGroupAsync(int userId, int groupId, CreateUserViewModel model = null);

        Task<bool> IsUserInGroup(int userId, int groupId);

        Task<bool> RemoveUserFromGroupAsync(int userId, int groupId);
    }
}