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

        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _logger = logger;
        
            _userRepository = userRepository;
        }
        public async Task<UserViewModel> CreateUserAsync(CreateUserViewModel model)
        {
            if (await _userRepository.UserExistsAsync(model.Email, model.Account))
            {
                _logger.LogWarning("User with Email {Email} and Account {Account} already exists", model.Email, model.Account);
                return null;
            }
            var user = model.ToEntityFromCreateUserViewModel();
            var createUser = await _userRepository.CreateUserAsync(user);
            if (createUser == null)
            {
                _logger.LogWarning("Failed to create user");
                return null;
            }
            return createUser.ToUserViewModelFromEntity();

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
            if (!await _userRepository.UserExistsAsync(model.Id))
            {
                _logger.LogWarning("User with Id {Id} does not exist", model.Id);
                return null;
            }
            if (await _userRepository.UserExistsAsync(model.Email, model.Account))
            {
                _logger.LogWarning("User with Email {Email} and Account {Account} already exists", model.Email, model.Account);
                return null;
            }
            var user = model.ToEntityFromUpdateUserViewModel();
            var updateUser = await _userRepository.UpdateUserAsync(user);
            if (updateUser == null)
            {
                throw new Exception("Update user failed");
            }
            return updateUser.ToUserViewModelFromEntity();

        }
    }
}