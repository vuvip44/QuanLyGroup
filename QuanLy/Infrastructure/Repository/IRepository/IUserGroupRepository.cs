using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.Infrastructure.Repository.IRepository
{
    public interface IUserGroupRepository
    {
        Task<int> AddUserToGroupAsync(int userId, int groupId);
        Task<int> RemoveUserFromGroupAsync(int userId, int groupId);

        Task<bool> IsUserInGroupAsync(int userId, int groupId);
    }
}