using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using QuanLy.Infrastructure.Repository.IRepository;
using QuanLy.Mapping;
using QuanLy.Models;
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

        private readonly ILogger<GroupService> _logger;

        public GroupService(IGroupRepository groupRepository, ILogger<GroupService> logger)
        {
            _groupRepository = groupRepository;
            _logger = logger;
        }
        public async Task<GroupViewModel> CreateGroupAsync(CreateGroupViewModel model)
        {
            if (await _groupRepository.GroupExistsAsync(model.Code, model.Name))
            {
                _logger.LogWarning("Group with Code {Code} already exists", model.Code);
                return null;
            }
            var group = model.ToEntityFromCreateGroup();
            var createdGroup = await _groupRepository.CreateGroupAsync(group);
            if (createdGroup == null)
            {
                _logger.LogWarning("Failed to create group with Code {Code}", model.Code);
                return null;
            }
            return createdGroup.ToGroupViewModelFromEntity();
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            if (!await _groupRepository.GroupExistsAsync(id))
            {
                _logger.LogWarning("Group with Id {Id} already exists", id);
                return false;
            }
            var result = await _groupRepository.DeleteGroupAsync(id);
            return result > 0;
        }

        public async Task<PageResponse<IEnumerable<GroupViewModel>>> GetAllGroupAsync(QueryObject query)
        {
            var pageResponse = await _groupRepository.GetAllGroupsAsync(query);
            var groupEntities = (IEnumerable<Group>)pageResponse.Data;
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

        public async Task<List<GroupViewModel>> GetAllGroupsAsync()
        {
            try
            {
                var groups = await _groupRepository.GetAllGroupsAsync();
                return groups.Select(g => new GroupViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Code = g.Code,
                    OrderNumber = g.OrderNumber,
                    ParentGroupId = g.ParentGroupId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all groups.");
                return new List<GroupViewModel>();
            }
        }

        public async Task<GroupViewModel?> GetGroupByIdAsync(int id)
        {
            if (!await _groupRepository.GroupExistsAsync(id))
            {
                _logger.LogWarning("Group with Id {Id} already exists", id);
                return null;
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
            var groupNodes = ((IEnumerable<Group>)groups).Select(g => g.ToGroupTreeNodeViewModelFromEntity()).ToList();
            var tree = new List<GroupTreeNodeViewModel>();
            var lookup = groupNodes.ToLookup(g => g.Id);
            foreach (var node in groupNodes)
            {
                if (node.ParentId.HasValue && lookup.Contains(node.ParentId.Value))
                {
                    var parentNode = lookup[node.ParentId.Value].FirstOrDefault();
                    if (parentNode != null)
                    {
                        parentNode.Children.Add(node);
                    }
                }
                else
                {
                    tree.Add(node);
                }
            }
            return tree.OrderBy(g => g.OrderNumber).ToList();
        }

        public async Task<PageResponse<IEnumerable<UserInGroupViewModel>>> GetGroupUsersAsync(int groupId, QueryObject query)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group == null)
            {
                _logger.LogWarning("Group with Id {Id} already exists", groupId);
                return null;
            }
            var usersInGroup = group.UserGroups?.Select(ug => ug.User).OrderBy(u => u.OrderNumber).ToList() ?? new List<User>();
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                usersInGroup = usersInGroup.Where(u => u.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            var totalCount = usersInGroup.Count();
            var skip = (query.Page - 1) * query.PageSize;
            var pageUsers = usersInGroup.Skip(skip).Take(query.PageSize).ToList();
            var userViewModel = new List<UserInGroupViewModel>();
            foreach (var user in pageUsers)
            {
                var userGroups = (user.UserGroups).Select(ug => ug.ToGroupInforViewModelFromUserGroup()).ToList();
                userViewModel.Add(new UserInGroupViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Account = user.Account,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    OrderNumber = user.OrderNumber,
                    Groups = userGroups
                });
            }
            return new PageResponse<IEnumerable<UserInGroupViewModel>>
            {
                Page = query.Page,
                PageSize = query.PageSize,
                TotalElement = totalCount,
                TotalPage = (long)Math.Ceiling((double)totalCount / query.PageSize),
                Data = userViewModel.AsEnumerable()
            };


        }

        public async Task<GroupViewModel> UpdateGroupAsync(UpdateGroupViewModel model)
        {
            var groupModel = await _groupRepository.GetGroupByIdAsync(model.Id);
            if (groupModel == null)
            {
                _logger.LogWarning("Group with Id {Id} already exists", model.Id);
                return null;
            }

            if (await _groupRepository.GroupExistsAsync(model.Code, model.Name))
            {
                _logger.LogWarning("Group with Code {Code} already exists", model.Code);
                return null;
            }
            var group = model.ToEntityFromUpdateGroup();
            group.UserGroups = groupModel?.UserGroups;
            group.Name = groupModel.Name;
            var updatedGroup = await _groupRepository.UpdateGroupAsync(group);
            if (updatedGroup == null)
            {
                _logger.LogWarning("Failed to update group with Id {Id}", model.Id);
                return null;
            }
            return updatedGroup.ToGroupViewModelFromEntity();
        }
    }
}