using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Models;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Response;

namespace QuanLy.Infrastructure.Repository.IRepository
{
    public interface IGroupRepository
    {
        Task<Group?> CreateGroupAsync(Group group);

        Task<Group?> GetGroupByIdAsync(int id);

        Task<Group> UpdateGroupAsync(Group group);

        Task<int> DeleteGroupAsync(int id);

        Task<PageResponse<IEnumerable<Group>>> GetAllGroupsAsync(QueryObject query);

        Task<bool> GroupExistsAsync(string codeGroup, string name);

        Task<bool> GroupExistsAsync(int id);
    }
}