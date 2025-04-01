using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuanLy.Infrastructure.Service.IService;
using QuanLy.Models;
using QuanLy.ViewModel.Require;
using QuanLy.ViewModel.Response.Group;
using QuanLy.ViewModel.Response.User;
using QuanLy.ViewModel.ViewMain;

namespace QuanLy.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGroupService _groupService;
    private readonly IUserService _userService;

    public HomeController(IGroupService groupService, IUserService userService, ILogger<HomeController> logger)
    {
        _groupService = groupService;
        _userService = userService;
        _logger = logger;
    }




    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

}
