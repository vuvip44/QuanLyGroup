using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanLy.Infrastructure.Repository.IRepository;
using QuanLy.Infrastructure.Service.IService;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Require.Group;
using QuanLy.ViewModel.Require.User;
using QuanLy.ViewModel.Response.Group;
using QuanLy.ViewModel.Response.User;
using QuanLy.ViewModel.ViewMain;

namespace QuanLy.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        private readonly IUserService _userService;

        private readonly IUserRepository _userRepository;

        private readonly ILogger<GroupController> _logger;

        public GroupController(IGroupService groupService, IUserService userService, IUserRepository userRepository, ILogger<GroupController> logger)
        {
            _groupService = groupService;
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
        }

        // // GET: /Group
        // public async Task<IActionResult> Index(QueryObject query)
        // {
        //     // Lấy danh sách tất cả nhóm
        //     var groups = await _groupService.GetAllGroupsAsync();
        //     var groupList = groups ?? new List<GroupViewModel>();

        //     // Chuyển đổi danh sách nhóm thành dữ liệu dạng cây
        //     var treeData = BuildGroupTree(groupList);
        //     ViewBag.GroupTreeData = System.Text.Json.JsonSerializer.Serialize(treeData);
        //     Console.WriteLine($"GroupTreeData: {ViewBag.GroupTreeData}");

        //     // Nếu không có groupId được chọn, chọn nhóm đầu tiên (nếu có)
        //     if (!query.GroupId.HasValue && groupList.Any())
        //     {
        //         query.GroupId = groupList.First().Id;
        //     }

        //     // Lấy danh sách người dùng thuộc nhóm được chọn
        //     GroupUsersViewModel selectedGroupUsers = null;
        //     if (query.GroupId.HasValue)
        //     {
        //         var usersResponse = await _userService.GetAllUsersAsync(query);
        //         var group = groupList.FirstOrDefault(g => g.Id == query.GroupId);

        //         // Ép kiểu usersResponse.Data thành IEnumerable<UserViewModel> trước khi gọi Select
        //         var userViewModels = usersResponse.Data as IEnumerable<UserViewModel> ?? Enumerable.Empty<UserViewModel>();

        //         selectedGroupUsers = new GroupUsersViewModel
        //         {
        //             GroupId = query.GroupId.Value,
        //             GroupName = group?.Name,
        //             GroupCode = group?.Code,
        //             Users = userViewModels.Select(u => new UserInGroupViewModel
        //             {
        //                 Id = u.Id,
        //                 Name = u.Name,
        //                 Email = u.Email,
        //                 Account = u.Account,
        //                 OrderNumber = u.OrderNumber,
        //                 BirthDate = u.BirthDate,
        //                 Gender = u.Gender,
        //                 PhoneNumber = u.PhoneNumber,
        //                 Groups = u.Groups
        //             }).ToList()
        //         };

        //         ViewBag.PageResponse = usersResponse;
        //         ViewBag.SearchName = query.Name;
        //     }

        //     // Tạo DashboardViewModel
        //     var dashboard = new DashboardViewModel
        //     {
        //         GroupTree = treeData,
        //         SelectedGroupUsers = selectedGroupUsers
        //     };

        //     ViewBag.SelectedGroupId = query.GroupId;
        //     return View(dashboard);
        // }


        // private List<GroupTreeNodeViewModel> BuildGroupTree(List<GroupViewModel> groups)
        // {
        //     var tree = new List<GroupTreeNodeViewModel>();
        //     var groupDict = groups.ToDictionary(g => g.Id, g => new GroupTreeNodeViewModel
        //     {
        //         Id = g.Id,
        //         Name = g.Name,
        //         ParentId = g.ParentGroupId,
        //         Code = g.Code,
        //         OrderNumber = g.OrderNumber,
        //         ICon = "jstree-folder",
        //         Children = new List<GroupTreeNodeViewModel>()
        //     });

        //     foreach (var group in groups)
        //     {
        //         var node = groupDict[group.Id];
        //         if (group.ParentGroupId.HasValue && groupDict.ContainsKey(group.ParentGroupId.Value))
        //         {
        //             groupDict[group.ParentGroupId.Value].Children.Add(node);
        //         }
        //         else
        //         {
        //             tree.Add(node);
        //         }
        //     }

        //     return tree;
        // }
        public async Task<IActionResult> Index(QueryObject query)
        {
            // Lấy danh sách tất cả nhóm
            var groups = await _groupService.GetAllGroupsAsync();
            var groupList = groups ?? new List<GroupViewModel>();

            // Truyền danh sách nhóm trực tiếp vào ViewBag
            ViewBag.GroupList = groupList;

            // Nếu không có groupId được chọn, chọn nhóm đầu tiên (nếu có)
            if (!query.GroupId.HasValue && groupList.Any())
            {
                query.GroupId = groupList.First().Id;
            }

            // Lấy danh sách người dùng thuộc nhóm được chọn
            GroupUsersViewModel selectedGroupUsers = null;
            if (query.GroupId.HasValue)
            {
                var usersResponse = await _userService.GetAllUsersAsync(query);
                var group = groupList.FirstOrDefault(g => g.Id == query.GroupId);

                // Ép kiểu usersResponse.Data thành IEnumerable<UserViewModel> trước khi gọi Select
                var userViewModels = usersResponse.Data as IEnumerable<UserViewModel> ?? Enumerable.Empty<UserViewModel>();

                selectedGroupUsers = new GroupUsersViewModel
                {
                    GroupId = query.GroupId.Value,
                    GroupName = group?.Name,
                    GroupCode = group?.Code,
                    Users = userViewModels.Select(u => new UserInGroupViewModel
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Account = u.Account,
                        OrderNumber = u.OrderNumber,
                        BirthDate = u.BirthDate,
                        Gender = u.Gender,
                        PhoneNumber = u.PhoneNumber,
                        Groups = u.Groups
                    }).ToList()
                };

                ViewBag.PageResponse = usersResponse;
                ViewBag.SearchName = query.Name;
            }

            // Tạo DashboardViewModel
            var dashboard = new DashboardViewModel
            {
                SelectedGroupUsers = selectedGroupUsers
            };

            ViewBag.SelectedGroupId = query.GroupId;
            return View(dashboard);
        }
        // GET: /Group/GetGroupTree
        [HttpGet]
        public async Task<IActionResult> GetGroupTree()
        {
            try
            {
                var tree = await _groupService.GetGroupTreeAsync();
                return Json(tree);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving group tree.");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        // GET: /Group/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _groupService.CreateGroupAsync(model);
                if (result == null)
                {
                    TempData["ErrorMessage"] = "Failed to create group. Group may already exist or an error occurred.";
                    return View(model);
                }

                TempData["SuccessMessage"] = "Group created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating group.");
                TempData["ErrorMessage"] = "An unexpected error occurred while creating group.";
                return View(model);
            }
        }

        // GET: /Group/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var group = await _groupService.GetGroupByIdAsync(id);
                if (group == null)
                {
                    TempData["ErrorMessage"] = "Group not found.";
                    return RedirectToAction(nameof(Index));
                }

                var model = new UpdateGroupViewModel
                {
                    Id = group.Id,
                    Name = group.Name,
                    Code = group.Code,
                    OrderNumber = group.OrderNumber,
                    ParentGroupId = group.ParentGroupId
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving group with Id {Id} for editing.", id);
                TempData["ErrorMessage"] = "An unexpected error occurred while retrieving group.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Group/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _groupService.UpdateGroupAsync(model);
                if (result == null)
                {
                    TempData["ErrorMessage"] = "Failed to update group. Group may not exist or an error occurred.";
                    return View(model);
                }

                TempData["SuccessMessage"] = "Group updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating group with Id {Id}.", model.Id);
                TempData["ErrorMessage"] = "An unexpected error occurred while updating group.";
                return View(model);
            }
        }

        // POST: /Group/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _groupService.DeleteGroupAsync(id);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Group not found.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Group deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting group with Id {Id}.", id);
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting group.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}