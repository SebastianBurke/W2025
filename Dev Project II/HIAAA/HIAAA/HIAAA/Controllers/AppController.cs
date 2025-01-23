using HIAAA.DAL;
using HIAAA.DAL.Interfaces;
using HIAAA.DTO;
using HIAAA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;


namespace HIAAA.Controllers;

public class AppController : Controller
{
    // GET
    private readonly IApp _appRepository;
    private readonly IAppAdminRepository _appAdminRepository;

    public AppController(IApp appRepository, IAppAdminRepository appAdminRepository)
    {
        _appRepository = appRepository;
        _appAdminRepository = appAdminRepository;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        try
        {
            var apps = await _appRepository.GetAppsAsync(); // Fetch all apps

            if (apps == null)
            {
                apps = new List<App>();
            }

            return View(apps); // Pass apps to the view
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"An error occurred while fetching apps: {ex.Message}";
            return View("Error"); // Render the error view with a message
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.AppTypeOptions = _appRepository.GetAppTypeOptions();
        ViewBag.AppAdmins = await _appAdminRepository.GetAppAdminsAsSelectListItems();
        
        return View(new AddAppDTO());
    }


    [HttpPost]
    public async Task<IActionResult> Create(AddAppDTO dto)
    {
        if (!ModelState.IsValid)
        {
            // Repopulate the dropdowns cleanly
            ViewBag.AppTypeOptions = _appRepository.GetAppTypeOptions();
            ViewBag.AppAdmins = await _appAdminRepository.GetAppAdminsAsSelectListItems();

            // Return the view with validation errors
            return View(dto);
        }

        try
        {
            var createdApp = await _appRepository.AddAppAsync(dto);

            // Assign the AppAdmin role
            await _appRepository.AssignAppToAppAdmin(createdApp.Appid, dto.AssignedAppAdminId);

            return RedirectToAction("Index", new { id = createdApp.Appid });
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError("Appcode", "App code must be unique!");

            // Repopulate the dropdowns cleanly again
            ViewBag.AppTypeOptions = _appRepository.GetAppTypeOptions();
            ViewBag.AppAdmins = await _appAdminRepository.GetAppAdminsAsSelectListItems();

            return View(dto);
        }
    }



    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        // Retrieve the app from the repository
        var app = await _appRepository.GetAppByIdAsync(id);
        if (app == null)
        {
            return NotFound($"App with ID {id} not found.");
        }

        // Map the app to UpdateAppDto
        var updateAppDto = new UpdateAppDTO()
        {
            AppId = app.Appid,
            AppCode = app.Appcode,
            AppName = app.Appname,
            Appdescription = app.Appdescription,
            Apptype = app.Apptype,
            CreatedBy = app.Createdby
        };

        // Populate the dropdown or other options
        ViewBag.AppTypeOptions = _appRepository.GetAppTypeOptions();

        // Return the view with the DTO
        return View(updateAppDto);
    }


    [HttpPost]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateAppDTO updateAppDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateAppDto); // Return the view with validation errors
        }

        try
        {
            var updatedApp = await _appRepository.UpdateAppAsync(updateAppDto);
            
            TempData["SuccessMessage"] = "App updated successfully.";
            return RedirectToAction("Index");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(updateAppDto);
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request errors
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(updateAppDto);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            // Call the repository method to delete the app
            bool isDeleted = await _appRepository.DeleteAppAsync(id);

            if (isDeleted)
            {
                TempData["SuccessMessage"] = "The app was successfully deleted.";
                return RedirectToAction("Index"); // Redirect to your desired view
            }

            TempData["ErrorMessage"] = "Failed to delete the app. Please try again.";
            return RedirectToAction("Index");
        }
        catch (HttpRequestException ex)
        {
            // Log the exception (optional)
            TempData["ErrorMessage"] = $"Error deleting the app: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}