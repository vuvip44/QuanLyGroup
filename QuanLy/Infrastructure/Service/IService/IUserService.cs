using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Require.User;
using QuanLy.ViewModel.Response;
using QuanLy.ViewModel.Response.User;

namespace QuanLy.Infrastructure.Service.IService
{
    public interface IUserService
    {
        Task<UserViewModel> CreateUserAsync(CreateUserViewModel model);
        Task<UserViewModel> UpdateUserAsync(UpdateUserViewModel model);
        Task<UserViewModel> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<PageResponse<IEnumerable<UserViewModel>>> GetAllUsersAsync(QueryObject query);
    }
}