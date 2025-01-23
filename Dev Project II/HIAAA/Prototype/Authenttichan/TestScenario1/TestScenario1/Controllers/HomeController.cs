using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using TestScenario1.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace TestScenario1.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Login()
        {
            TempData["Logged"] = "notLogged";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            LoginViewModel loginData = new LoginViewModel(username, password);
            if (!ModelState.IsValid)
            {
                return View(loginData);
            }
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var loginUrl = "http://localhost:12461/api/authenticate/1";
            var content = new StringContent(JsonConvert.SerializeObject(loginData), System.Text.Encoding.UTF8, "application/json");
            
            HttpResponseMessage res =  await _httpClient.PostAsync(loginUrl, content);
            if (res.IsSuccessStatusCode) {
                var data = await res.Content.ReadAsStringAsync();
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(data);
                return RedirectToAction("Subview", new { tokenStr = token.Token});
            }
            ModelState.AddModelError("Password", " ");
            return View(loginData);
        }

        public async Task<IActionResult> SubView(string tokenStr)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var rolesUrl = $"http://localhost:12461/api/authorize/{tokenStr}";
            HttpResponseMessage res = await _httpClient.PostAsync(rolesUrl, null);
            if (res.IsSuccessStatusCode) {
                TempData["Logged"] = "logged";
                string data = await res.Content.ReadAsStringAsync();
                List<UserRole> userRoles = JsonConvert.DeserializeObject<List<UserRole>>(data);
                return View(userRoles);
            }
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
