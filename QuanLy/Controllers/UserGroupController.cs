using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanLy.Infrastructure.Service.IService;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Require.User;

namespace QuanLy.Controllers
{
    public class UserGroupController : Controller
    {
        private readonly IUserGroupService _userGroupService;
        private readonly IGroupService _groupService;
        private readonly ILogger<UserGroupController> _logger;

        public UserGroupController(
            IUserGroupService userGroupService,
            IGroupService groupService,
            ILogger<UserGroupController> logger)
        {
            _userGroupService = userGroupService;
            _groupService = groupService;
            _logger = logger;
        }

        // GET: /UserGroup/GetUsersInGroup/5
        [HttpGet]
        public async Task<IActionResult> GetUsersInGroup(int groupId, [FromQuery] QueryObject query)
        {
            try
            {
                var users = await _groupService.GetGroupUsersAsync(groupId, query);
                if (users == null)
                {
                    return Json(new { success = false, message = "Group not found." });
                }

                return Json(new { success = true, data = users });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving users in group with Id {GroupId}.", groupId);
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

        public async Task<IActionResult> AddUserToGroup(int groupId)
        {
            try
            {
                var group = await _groupService.GetGroupByIdAsync(groupId);
                if (group == null)
                {
                    TempData["ErrorMessage"] = "Group not found.";
                    return RedirectToAction("Index", "Group");
                }

                ViewBag.GroupId = groupId;
                ViewBag.CurrentGroup = group; // Truyền thông tin nhóm hiện tại vào ViewBag
                return View(new CreateUserViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while preparing to add user to group with Id {GroupId}.", groupId);
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return RedirectToAction("Index", "Group");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToGroup(int groupId, int userId, CreateUserViewModel userModel = null)
        {
            try
            {
                // Nếu userId là 0 (tạo người dùng mới), kiểm tra validation của userModel
                if (userId == 0)
                {
                    if (userModel == null)
                    {
                        TempData["ErrorMessage"] = "Please provide details for a new user.";
                        ViewBag.GroupId = groupId;
                        var group = await _groupService.GetGroupByIdAsync(groupId);
                        ViewBag.CurrentGroup = group;
                        return View(new CreateUserViewModel());
                    }

                    if (!ModelState.IsValid)
                    {
                        ViewBag.GroupId = groupId;
                        var group = await _groupService.GetGroupByIdAsync(groupId);
                        ViewBag.CurrentGroup = group;
                        return View(userModel);
                    }
                }
                else
                {
                    // Nếu userId được cung cấp, bỏ qua userModel
                    userModel = null;
                }

                var result = await _userGroupService.AddUserToGroupAsync(userId, groupId, userModel);
                TempData["SuccessMessage"] = "User added to group successfully.";
                return RedirectToAction("Index", "Group");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding user to group with Id {GroupId}.", groupId);
                TempData["ErrorMessage"] = ex.Message;
                ViewBag.GroupId = groupId;
                var group = await _groupService.GetGroupByIdAsync(groupId);
                ViewBag.CurrentGroup = group;
                return View(userModel ?? new CreateUserViewModel());
            }
        }

        // POST: /UserGroup/RemoveUserFromGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromGroup(int userId, int groupId)
        {
            try
            {
                var result = await _userGroupService.RemoveUserFromGroupAsync(userId, groupId);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Failed to remove user from group. User may not be in the group.";
                    return RedirectToAction("Index", "Group", new { GroupId = groupId });
                }

                TempData["SuccessMessage"] = "User removed from group successfully.";
                return RedirectToAction("Index", "Group", new { GroupId = groupId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while removing user {UserId} from group {GroupId}.", userId, groupId);
                TempData["ErrorMessage"] = "An unexpected error occurred while removing user from group.";
                return RedirectToAction("Index", "Group", new { GroupId = groupId });
            }
        }
    }
}