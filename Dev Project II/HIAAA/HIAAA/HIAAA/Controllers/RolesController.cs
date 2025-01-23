using AspNetCoreHero.ToastNotification.Abstractions;
using HIAAA.DAL.Interfaces;
using HIAAA.DTO;
using HIAAA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIAAA.Controllers;

[Authorize(Roles = "APPADMIN,ADMIN")]
public class RolesController : Controller
{
    private readonly IRole _roleRepository;
    private readonly IApp _appRepository;
    private readonly INotyfService _notyfService;

    public RolesController(IRole roleRepository, IApp appRepository, INotyfService notyfService)
    {
        _roleRepository = roleRepository;
        _appRepository = appRepository;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _roleRepository.ReadAllRoles());
    }

    [HttpGet]
    public async Task<IActionResult> Create(int id)
    {
        var app = await _appRepository.GetApp(id);
        if (app == null)
            return NotFound();

        return View(new RoleDTO() { AppId = app.Appid, App = new AppDTO() {AppId = app.Appid, AppCode = app.Appcode, AppName = app.Appname, CreatedBy = app.Createdby}});
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleDTO newRoleDTO)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            errorMessages.Add("Errors:");
            errorMessages.Reverse();

            string combinedErrorMessage = string.Join("<br>", errorMessages);
            _notyfService.Error(combinedErrorMessage);

            return View(newRoleDTO);
        }

        if (!await _roleRepository.CreateRole(newRoleDTO.AppId, newRoleDTO))
            return View(newRoleDTO);

        _notyfService.Success("App role successfully created!");
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var role = await _roleRepository.ReadRole(id);
        if (role == null)
            return NotFound();
        
        return View(role);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoleDTO updatedRoleDTO)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            errorMessages.Add("Errors:");
            errorMessages.Reverse();

            string combinedErrorMessage = string.Join("<br>", errorMessages);
            _notyfService.Error(combinedErrorMessage);

            return View(updatedRoleDTO);
        }
        
        if (!await _roleRepository.UpdateRole(updatedRoleDTO))
            return View(updatedRoleDTO);

        _notyfService.Success("App role successfully created!");
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _roleRepository.DeleteRole(id))
            _notyfService.Error("Role contains users");
        else
            _notyfService.Success("Role successfully deleted!");

        return RedirectToAction("Index");
    }
}