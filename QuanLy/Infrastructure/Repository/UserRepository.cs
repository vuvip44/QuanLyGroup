using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLy.Data;
using QuanLy.Infrastructure.Repository.IRepository;
using QuanLy.Models;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Response;

namespace QuanLy.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDBContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User?> CreateUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in Repository CreateUserAsync");
                return null;
            }
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in Repository DeleteUserAsync");
                return 0;
            }
        }

        public async Task<PageResponse<IEnumerable<User>>> GetAllUsersAsync(QueryObject query)
        {
            var users = _context.Users.Include(u => u.UserGroups).ThenInclude(ug => ug.Group).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                users = users.Where(x => x.Name.Contains(query.Name));
            }
            if (query.GroupId.HasValue)
            {
                users = users.Where(x => x.UserGroups.Any(ug => ug.GroupId == query.GroupId));
            }

            users = users.OrderBy(u => u.OrderNumber);
            var totalCount = await users.CountAsync();
            var skip = (query.Page - 1) * query.PageSize;
            var result = await users.Skip(skip).Take(query.PageSize).ToListAsync();

            PageResponse<IEnumerable<User>> pageResponse = new PageResponse<IEnumerable<User>>
            {
                Page = query.Page,
                PageSize = query.PageSize,
                TotalElement = totalCount,
                TotalPage = (long)Math.Ceiling((double)totalCount / query.PageSize),
                Data = result
            };
            return pageResponse;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users.Include(u => u.UserGroups).ThenInclude(ug => ug.Group).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in Repository GetUserByIdAsync");
                return null;
            }
        }


        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in Repository UpdateUserAsync");
                return null;

            }
        }

        public async Task<bool> UserExistsAsync(string email, string account)
        {
            return await _context.Users.AnyAsync(x => x.Email == email || x.Account == account);
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(x => x.Id == id);
        }
    }
}