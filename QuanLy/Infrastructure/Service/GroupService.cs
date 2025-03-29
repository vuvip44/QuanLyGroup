using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuanLy.Infrastructure.Repository.IRepository;
using QuanLy.Mapping;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Require.Group;
using QuanLy.ViewModel.Response;
using QuanLy.ViewModel.Response.Group;
using QuanLy.ViewModel.ViewMain;

namespace QuanLy.Infrastructure.Service.IService
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task<GroupViewModel> CreateGroupAsync(CreateGroupViewModel model)
        {
            if (await _groupRepository.GroupExistsAsync(model.Code, model.Name))
            {
                throw new Exception("Group already exists.");
            }
            var group = model.ToEntityFromCreateGroup();
            var createdGroup = await _groupRepository.CreateGroupAsync(group);
            if (createdGroup == null)
            {
                throw new Exception("Failed to create group.");
            }
            return createdGroup.ToGroupViewModelFromEntity();
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            if (!await _groupRepository.GroupExistsAsync(id))
            {
                throw new Exception("Group not found.");
            }
            var result = await _groupRepository.DeleteGroupAsync(id);
            return result > 0;
        }

        public async Task<PageResponse<IEnumerable<GroupViewModel>>> GetAllGroupAsync(QueryObject query)
        {
            var pageResponse = await _groupRepository.GetAllGroupsAsync(query);
            var groupEntities = (IEnumerable<QuanLy.Models.Group>)pageResponse.Data;
            var groupViewModels = groupEntities.Select(g => g.ToGroupViewModelFromEntity()).ToList();
            var pageResponseViewModel = new PageResponse<IEnumerable<GroupViewModel>>
            {
                Data = groupViewModels,
                TotalElement = pageResponse.TotalElement,
                PageSize = pageResponse.PageSize,
                Page = pageResponse.Page
            };
            return pageResponseViewModel;
        }

        public async Task<GroupViewModel?> GetGroupByIdAsync(int id)
        {
            if (!await _groupRepository.GroupExistsAsync(id))
            {
                throw new Exception("Group not found.");
            }
            return (await _groupRepository.GetGroupByIdAsync(id))?.ToGroupViewModelFromEntity();
        }

        public async Task<List<GroupTreeNodeViewModel>> GetGroupTreeAsync()
        {
            var query = new QueryObject()
            {
                Page = 1,
                PageSize = 20
            };
            var groups = (await _groupRepository.GetAllGroupsAsync(query)).Data;
            // var groupNode=groups.Select(global=> new GroupTreeNodeViewModel()
            // {
            //     Id=global.Id,
            //     Name=global.Name,
            //     Code=global.Code,
            //     OrderNumber=global.OrderNumber,
            //     ParentGroupId=global.ParentGroupId
            // }).ToList();
            return null;
        }

        public Task<GroupUsersViewModel?> GetGroupUsersAsync(int groupId)
        {
            throw new NotImplementedException();
        }

        public Task<GroupViewModel> UpdateGroupAsync(UpdateGroupViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}