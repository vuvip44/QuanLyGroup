using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLy.Models;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Response;

namespace QuanLy.Infrastructure.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User?> CreateUserAsync(User user);

        Task<User?> GetUserByIdAsync(int id);

        Task<User> UpdateUserAsync(User user);

        Task<int> DeleteUserAsync(int id);

        Task<PageResponse<IEnumerable<User>>> GetAllUsersAsync(QueryObject query);

        Task<bool> UserExistsAsync(string email, string account);

        Task<bool> UserExistsAsync(int id);


    }
}