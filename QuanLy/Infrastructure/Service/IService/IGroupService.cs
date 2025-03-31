using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Require.Group;
using QuanLy.ViewModel.Response;
using QuanLy.ViewModel.Response.Group;
using QuanLy.ViewModel.ViewMain;

namespace QuanLy.Infrastructure.Service.IService
{
    public interface IGroupService
    {
        Task<List<GroupViewModel>> GetAllGroupsAsync();
        Task<GroupViewModel> CreateGroupAsync(CreateGroupViewModel model);
        Task<GroupViewModel> UpdateGroupAsync(UpdateGroupViewModel model);
        Task<bool> DeleteGroupAsync(int id);
        Task<PageResponse<IEnumerable<GroupViewModel>>> GetAllGroupAsync(QueryObject query);
        Task<GroupViewModel?> GetGroupByIdAsync(int id);
        Task<List<GroupTreeNodeViewModel>> GetGroupTreeAsync();
        Task<PageResponse<IEnumerable<UserInGroupViewModel>>> GetGroupUsersAsync(int groupId, QueryObject query);

    }
}