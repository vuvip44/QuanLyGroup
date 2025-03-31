using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Infrastructure.Repository.IRepository;
using QuanLy.Infrastructure.Service.IService;
using QuanLy.Mapping;
using QuanLy.Models;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Require.User;
using QuanLy.ViewModel.Response;
using QuanLy.ViewModel.Response.User;

namespace QuanLy.Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IUserGroupRepository _userGroupRepository;

        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IUserGroupRepository userGroupRepository)
        {
            _logger = logger;
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
        }


        public async Task<UserViewModel> CreateUserAsync(CreateUserViewModel model)
        {
            _logger.LogInformation("Starting to create user: Email={Email}, Account={Account}", model.Email, model.Account);

            if (await _userRepository.UserExistsAsync(model.Email, model.Account))
            {
                _logger.LogWarning("User with Email {Email} and Account {Account} already exists", model.Email, model.Account);
                return null;
            }

            var user = model.ToEntityFromCreateUserViewModel();
            _logger.LogInformation("User entity created: Name={Name}, Email={Email}, Account={Account}", user.Name, user.Email, user.Account);

            var createdUser = await _userRepository.CreateUserAsync(user);
            if (createdUser == null)
            {
                _logger.LogWarning("Failed to create user with Email={Email}, Account={Account}", model.Email, model.Account);
                return null;
            }

            _logger.LogInformation("User created successfully with Id={Id}", createdUser.Id);

            // Thêm user vào các nhóm (nếu có)
            if (model.GroupIds != null && model.GroupIds.Any())
            {
                foreach (var groupId in model.GroupIds)
                {
                    var result = await _userGroupRepository.AddUserToGroupAsync(createdUser.Id, groupId);
                    if (result == 0)
                    {
                        _logger.LogWarning("Failed to add user {UserId} to group {GroupId}", createdUser.Id, groupId);
                    }
                }
            }

            // Lấy lại user từ cơ sở dữ liệu để bao gồm thông tin nhóm
            var userWithGroups = await _userRepository.GetUserByIdAsync(createdUser.Id);
            if (userWithGroups == null)
            {
                _logger.LogWarning("Failed to retrieve user with groups after creation, UserId={UserId}", createdUser.Id);
                return createdUser.ToUserViewModelFromEntity();
            }

            return userWithGroups.ToUserViewModelFromEntity();

        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (!await _userRepository.UserExistsAsync(id))
            {
                _logger.LogWarning("User with Id {Id} does not exist", id);
                return false;

            }
            var result = await _userRepository.DeleteUserAsync(id);
            return result > 0;
        }

        public async Task<PageResponse<IEnumerable<UserViewModel>>> GetAllUsersAsync(QueryObject query)
        {
            var pageResponse = await _userRepository.GetAllUsersAsync(query);
            var userEntities = (IEnumerable<User>)pageResponse.Data;
            var userViewModels = userEntities.Select(u => u.ToUserViewModelFromEntity()).ToList();
            var pageResponseViewModel = new PageResponse<IEnumerable<UserViewModel>>
            {
                Page = pageResponse.Page,
                PageSize = pageResponse.PageSize,
                TotalElement = pageResponse.TotalElement,
                TotalPage = pageResponse.TotalPage,
                Data = userViewModels
            };
            return pageResponseViewModel;
        }

        public async Task<UserViewModel> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with Id {Id} does not exist", id);
                return null;
            }
            return user.ToUserViewModelFromEntity();
        }

        public async Task<UserViewModel> UpdateUserAsync(UpdateUserViewModel model)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(model.Id);
            if (existingUser == null)
            {
                _logger.LogWarning("User with Id {Id} does not exist", model.Id);
                return null;
            }




            // Cập nhật thông tin user
            existingUser.Name = model.Name;
            existingUser.Email = model.Email;
            existingUser.Account = model.Account;
            existingUser.OrderNumber = model.OrderNumber;
            existingUser.BirthDate = model.BirthDate;
            existingUser.Gender = model.Gender;
            existingUser.PhoneNumber = model.PhoneNumber;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            if (updatedUser == null)
            {
                _logger.LogWarning("Failed to update user with Id={Id}", model.Id);
                return null;
            }

            // Cập nhật danh sách nhóm
            // Bước 1: Xóa tất cả UserGroups hiện tại của user
            await _userGroupRepository.RemoveUserFromAllGroupsAsync(updatedUser.Id);

            // Bước 2: Thêm user vào các nhóm mới
            if (model.GroupIds != null && model.GroupIds.Any())
            {
                foreach (var groupId in model.GroupIds)
                {
                    var result = await _userGroupRepository.AddUserToGroupAsync(updatedUser.Id, groupId);
                    if (result == 0)
                    {
                        _logger.LogWarning("Failed to add user {UserId} to group {GroupId}", updatedUser.Id, groupId);
                    }
                }
            }

            // Lấy lại user từ cơ sở dữ liệu để bao gồm thông tin nhóm
            var userWithGroups = await _userRepository.GetUserByIdAsync(updatedUser.Id);
            if (userWithGroups == null)
            {
                _logger.LogWarning("Failed to retrieve user with groups after update, UserId={UserId}", updatedUser.Id);
                return updatedUser.ToUserViewModelFromEntity();
            }

            return userWithGroups.ToUserViewModelFromEntity();

        }
    }
}