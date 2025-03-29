using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLy.Data;
using QuanLy.Models;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Response;

namespace QuanLy.Infrastructure.Repository.IRepository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDBContext _context;

        private readonly ILogger<GroupRepository> _logger;

        public GroupRepository(ApplicationDBContext context, ILogger<GroupRepository> logger)
        {
            _context = context;
            _logger = logger;
        }



        public async Task<Group?> CreateGroupAsync(Group group)
        {
            try
            {
                await _context.Groups.AddAsync(group);
                await _context.SaveChangesAsync();
                return group;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in Repository CreateGroupAsync");
                return null;
            }
        }

        public async Task<int> DeleteGroupAsync(int id)
        {
            try
            {
                var group = await _context.Groups.FindAsync(id);
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in Repository DeleteGroupAsync");
                return 0;
            }

        }

        public async Task<PageResponse<IEnumerable<Group>>> GetAllGroupsAsync(QueryObject query)
        {
            var groups = _context.Groups.Include(g => g.UserGroups).ThenInclude(ug => ug.User).Include(g => g.ParentGroup).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.GroupName))
            {
                groups = groups.Where(g => g.Name.Contains(query.GroupName));
            }
            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                groups = groups.Where(g => g.Code.Contains(query.Code));
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                groups = groups.Where(g => g.UserGroups.Any(ug => ug.User.Name.Contains(query.Name)));
            }

            groups = groups.OrderBy(g => g.OrderNumber);
            var totalCount = await groups.CountAsync();
            var skip = (query.Page - 1) * query.PageSize;
            var result = await groups.Skip(skip).Take(query.PageSize).ToListAsync();
            PageResponse<IEnumerable<Group>> pageResponse = new PageResponse<IEnumerable<Group>>
            {
                Page = query.Page,
                PageSize = query.PageSize,
                TotalElement = totalCount,
                TotalPage = (long)Math.Ceiling((double)totalCount / query.PageSize),
                Data = result
            };
            return pageResponse;
        }

        public async Task<Group?> GetGroupByIdAsync(int id)
        {
            try
            {
                return await _context.Groups.Include(g => g.ParentGroup).Include(g => g.UserGroups).ThenInclude(ug => ug.User).FirstOrDefaultAsync(g => g.Id == id);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in Repository GetGroupByIdAsync");
                return null;

            }
        }

        public async Task<bool> GroupExistsAsync(string codeGroup, string name)
        {
            return await _context.Groups.AnyAsync(x => x.Code == codeGroup || x.Name == name);
        }

        public Task<bool> GroupExistsAsync(int id)
        {
            return _context.Groups.AnyAsync(x => x.Id == id);
        }

        public async Task<Group> UpdateGroupAsync(Group group)
        {
            try
            {
                _context.Update(group);
                await _context.SaveChangesAsync();
                return group;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error in Repository UpdateGroupAsync");
                return null;
            }
        }
    }
}