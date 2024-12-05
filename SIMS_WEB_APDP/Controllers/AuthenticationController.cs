using Microsoft.AspNetCore.Mvc;
using SIMS_WEB_APDP.Models;
using System.Text.Json;
using System;

public class AuthenticationController : Controller
{
    private const string FilePath = "wwwroot/data/user.json";

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;  // Lưu trữ URL mà người dùng muốn truy cập
        return View();
    }

    [HttpPost]
    public IActionResult Login(User user, string returnUrl = null)
    {
        List<User>? users = ReadFileToUserList();
        var result = users.FirstOrDefault(u => u.UserName == user.UserName && u.PassWord == user.PassWord);
        if (result != null)
        {
            HttpContext.Session.SetString("UserName", result.UserName);
            HttpContext.Session.SetString("Role", result.Role);

            // Điều hướng về URL mà người dùng muốn truy cập sau khi đăng nhập thành công
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Student");
            }
        }
        else
        {
            ViewBag.error = "Invalid user";
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {
            List<User>? users = ReadFileToUserList();

            if (users.Any(u => u.UserName == newUser.UserName))
            {
                ViewBag.error = "Username already exists";
                return View();
            }

            newUser.Id = (users.Count > 0 ? users.Max(u => u.Id) : 0) + 1;
            newUser.Role = "User";
            users.Add(newUser);

            try
            {
                string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(FilePath, json);
                ViewBag.message = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.error = "An error occurred while saving user data: " + ex.Message;
                return View();
            }
        }
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Authentication");
    }

    private List<User>? ReadFileToUserList()
    {
        if (System.IO.File.Exists(FilePath))
        {
            string readText = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(readText) ?? new List<User>();
        }
        return new List<User>();
    }
}
