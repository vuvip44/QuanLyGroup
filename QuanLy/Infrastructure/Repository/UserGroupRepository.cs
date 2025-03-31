using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLy.Data;
using QuanLy.Infrastructure.Repository.IRepository;
using QuanLy.Models;

namespace QuanLy.Infrastructure.Repository
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly ApplicationDBContext _context;

        private readonly ILogger<UserGroupRepository> _logger;

        public UserGroupRepository(ApplicationDBContext context, ILogger<UserGroupRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddUserToGroupAsync(int userId, int groupId)
        {
            try
            {
                var userGroup = new UserGroup
                {
                    UserId = userId,
                    GroupId = groupId
                };
                await _context.UserGroups.AddAsync(userGroup);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in UserGroupRepository AddUserToGroupAsync for User {UserId} and Group {GroupId}", userId, groupId);
                return 0;
            }
        }

        public async Task<bool> IsUserInGroupAsync(int userId, int groupId)
        {
            return await _context.UserGroups.AnyAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
        }

        public async Task<bool> RemoveUserFromAllGroupsAsync(int userId)
        {
            try
            {
                var userGroups = await _context.UserGroups
                    .Where(ug => ug.UserId == userId)
                    .ToListAsync();

                if (userGroups.Any())
                {
                    _context.UserGroups.RemoveRange(userGroups);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Removed user {UserId} from all groups", userId);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error removing user {UserId} from all groups", userId);
                return false;
            }
        }

        public async Task<int> RemoveUserFromGroupAsync(int userId, int groupId)
        {
            try
            {
                var userGroup = await _context.UserGroups.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
                _context.UserGroups.Remove(userGroup);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in UserGroupRepository RemoveUserFromGroupAsync for User {UserId} and Group {GroupId}", userId, groupId);
                return 0;
            }
        }
    }
}