using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLy.Infrastructure.Service.IService;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Require.User;
using QuanLy.ViewModel.Response;
using QuanLy.ViewModel.Response.User;

namespace QuanLy.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IGroupService _groupService;

        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger, IGroupService groupService)
        {
            _userService = userService;
            _groupService = groupService;
            _logger = logger;
        }

        public async Task<IActionResult> Create()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            ViewBag.Groups = groups;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return View(model);
            }

            try
            {
                var result = await _userService.CreateUserAsync(model);
                if (result == null)
                {
                    TempData["ErrorMessage"] = "Failed to create user. User may already exist or an error occurred.";
                    return View(model);
                }
                TempData["SuccessMessage"] = "User created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating user.");
                TempData["ErrorMessage"] = "An unexpected error occurred while creating user.";
                return View(model);
            }
        }

        //GET: /Users/Edit/1

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }
                var model = new UpdateUserViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Gender = user.Gender,
                    Account = user.Account,
                    PhoneNumber = user.PhoneNumber,
                    OrderNumber = user.OrderNumber,
                    GroupIds = user.Groups.Select(g => g.Id).ToList()
                };
                var groups = await _groupService.GetAllGroupsAsync();
                ViewBag.Groups = groups;
                return View(model);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving user with Id {Id} for editing.", id);
                TempData["ErrorMessage"] = "An unexpected error occurred while retrieving user.";
                return RedirectToAction(nameof(Index));
            }
        }

        //POST: /users/edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _userService.UpdateUserAsync(model);
                if (result == null)
                {
                    TempData["ErrorMessage"] = "Failed to update user. User may not exist or an error occurred.";
                    return View(model);
                }
                TempData["SuccessMessage"] = "User updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating user with Id {Id}.", model.Id);
                TempData["ErrorMessage"] = "An unexpected error occurred while updating user.";
                return View(model);
            }
        }

        //POST: /users/Delete/1
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["SuccessMessage"] = "User deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting user with Id {Id}.", id);
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting user.";
                return RedirectToAction(nameof(Index));
            }
        }

        //GET: /users
        public async Task<IActionResult> Index(QueryObject query)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(query);
                return View(users);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving users.");
                TempData["ErrorMessage"] = "An unexpected error occurred while retrieving users.";
                return View(new PageResponse<IEnumerable<UserViewModel>>());
            }
        }

        //GET: users/Details/1
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(user);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving user with Id {Id}.", id);
                TempData["ErrorMessage"] = "An unexpected error occurred while retrieving user.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}