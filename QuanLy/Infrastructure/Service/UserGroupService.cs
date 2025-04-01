using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Infrastructure.Repository.IRepository;
using QuanLy.ViewModel.Require.User;

namespace QuanLy.Infrastructure.Service.IService
{
    public class UserGroupService : IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepository;

        private readonly IUserRepository _userRepository;

        private readonly IGroupRepository _groupRepository;

        private readonly IUserService _userService;

        private readonly ILogger<UserGroupService> _logger;

        public UserGroupService(
            IUserGroupRepository userGroupRepository,
            IUserRepository userRepository,
            IGroupRepository groupRepository,
            IUserService userService,
            ILogger<UserGroupService> logger)
        {
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _userService = userService;
            _logger = logger;
        }

        public async Task<bool> AddUserToGroupAsync(int userId, int groupId, CreateUserViewModel model = null)
        {
            try
            {
                // Kiểm tra sự tồn tại của nhóm
                if (!await _groupRepository.GroupExistsAsync(groupId))
                {
                    _logger.LogWarning("Group with Id {GroupId} does not exist.", groupId);
                    throw new Exception("Group does not exist.");
                }

                // Kiểm tra sự tồn tại của người dùng
                if (!await _userRepository.UserExistsAsync(userId))
                {
                    if (model == null)
                    {
                        _logger.LogWarning("User with Id {UserId} does not exist and no user model provided to create a new user.", userId);
                        throw new Exception("User does not exist and no user model provided to create a new user.");
                    }

                    // Tạo người dùng mới
                    var createdUser = await _userService.CreateUserAsync(model);
                    if (createdUser == null)
                    {
                        _logger.LogWarning("Failed to create new user with Email {Email} and Account {Account}.", model.Email, model.Account);
                        throw new Exception("Failed to create new user.");
                    }

                    userId = createdUser.Id;
                    _logger.LogInformation("Created new user with Id {UserId} for adding to Group {GroupId}.", userId, groupId);
                }

                // Kiểm tra xem người dùng đã thuộc nhóm hay chưa
                if (await _userGroupRepository.IsUserInGroupAsync(userId, groupId))
                {
                    _logger.LogWarning("User {UserId} is already in Group {GroupId}.", userId, groupId);
                    throw new Exception("User is already in the group.");
                }

                // Thêm người dùng vào nhóm
                var result = await _userGroupRepository.AddUserToGroupAsync(userId, groupId);
                if (result == 0)
                {
                    _logger.LogWarning("Failed to add User {UserId} to Group {GroupId}.", userId, groupId);
                    throw new Exception("Failed to add user to group.");
                }

                _logger.LogInformation("Successfully added User {UserId} to Group {GroupId}.", userId, groupId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user {UserId} to group {GroupId}.", userId, groupId);
                throw new Exception(ex.Message, ex);
            }
        }



        public async Task<bool> IsUserInGroup(int userId, int groupId)
        {
            return await _userGroupRepository.IsUserInGroupAsync(userId, groupId);
        }

        public async Task<bool> RemoveUserFromGroupAsync(int userId, int groupId)
        {
            // Kiểm tra sự tồn tại của người dùng
            if (!await _userRepository.UserExistsAsync(userId))
            {
                _logger.LogWarning("User with Id {UserId} does not exist.", userId);
                return false;
            }

            // Kiểm tra sự tồn tại của nhóm
            if (!await _groupRepository.GroupExistsAsync(groupId))
            {
                _logger.LogWarning("Group with Id {GroupId} does not exist.", groupId);
                return false;
            }

            // Kiểm tra xem người dùng có thuộc nhóm hay không
            if (!await _userGroupRepository.IsUserInGroupAsync(userId, groupId))
            {
                _logger.LogWarning("User {UserId} is not in Group {GroupId}.", userId, groupId);
                return false;
            }

            // Xóa người dùng khỏi nhóm
            var result = await _userGroupRepository.RemoveUserFromGroupAsync(userId, groupId);
            if (result == 0)
            {
                _logger.LogWarning("Failed to remove User {UserId} from Group {GroupId}.", userId, groupId);
                return false;
            }

            return true;
        }
    }
}