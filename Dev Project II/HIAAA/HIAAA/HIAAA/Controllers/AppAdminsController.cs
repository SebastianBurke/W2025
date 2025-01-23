using HIAAA.DAL;
using HIAAA.DTO;
using HIAAA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIAAA.Controllers {
    [Authorize(Roles = "ADMIN")]
    public class AppAdminsController : Controller
    {
        private readonly IAppAdminRepository _appAdminRepo;

        public AppAdminsController(IAppAdminRepository appAdminRepo)
        {
            _appAdminRepo = appAdminRepo;
        }
        
        public async Task<IActionResult> Index()
        {
            TempData["Success"] = "";
            var appAdmins = await _appAdminRepo.GetAll();
            return View(appAdmins);
        }

        public async Task<IActionResult> Details(int? id)
        {
            return View(); // TODO: display details about the app(s) that this app admin manages
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id != null)
            {
                var appAdmin = await _appAdminRepo.GetById((long)id);
                return View(new AppAdminDTO(appAdmin));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppAdminDTO appAdmin, string? password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appAdminRepo.Update(appAdmin);
                    if (password != null)
                        await _appAdminRepo.SetPassword(appAdmin, password);
                    TempData["Success"] = "App admin updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["Error"] = e.Message;
                    return View(appAdmin);
                }
            }
            return View(appAdmin);
        }
        
        public async Task<IActionResult> Delete(int id) {
            // TODO: Add confirmation popup for deletion "Are you sure?"
            try
            {
                await _appAdminRepo.Delete(id);
                TempData["Success"] = "App Admin deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { message = e.Message });
            }
        }
    }
}
