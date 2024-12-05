using Microsoft.AspNetCore.Mvc;
using SIMS_WEB_APDP.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Json;

namespace SIMS_WEB_APDP.Controllers
{
    public class StudentController : Controller
    {
        private static List<Student> students = new List<Student>
        {
            new Student { Id = 1, FullName = "Nguyen Van A", BirthYear = new DateTime(2000, 1, 1), Gender = "Male", PhoneNumber = "0123456789", Email = "a@example.com", Major = "Computer Science" },
            new Student { Id = 2, FullName = "Nguyen Thi B", BirthYear = new DateTime(2001, 2, 2), Gender = "Female", PhoneNumber = "0987654321", Email = "b@example.com", Major = "Information Technology" },
            new Student { Id = 3, FullName = "Nguyen Van C", BirthYear = new DateTime(2004, 2, 2), Gender = "Male", PhoneNumber = "0366144145", Email = "c@example.com", Major = "Marketing" }
        };

        // Kiểm tra trạng thái đăng nhập
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetString("UserName") != null;
        }

        public IActionResult Index(string searchString)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var filteredStudents = string.IsNullOrEmpty(searchString)
                ? students
                : students.Where(s => s.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                      s.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                      s.PhoneNumber.Contains(searchString)).ToList();

            ViewData["CurrentFilter"] = searchString; // Lưu từ khóa tìm kiếm vào ViewData
            return View(filteredStudents);
        }

        public IActionResult Create()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Kiểm tra trùng lặp email
            if (students.Any(s => s.Email == student.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
            }

            // Kiểm tra trùng lặp số điện thoại
            if (students.Any(s => s.PhoneNumber == student.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Số điện thoại đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                student.Id = students.Count + 1;
                students.Add(student);
                // Lưu danh sách sinh viên vào file student.json
                SaveStudentsToFile();

                return RedirectToAction("Index");
            }
            return View(student);
        }


        private void SaveStudentsToFile()
        {
            // Đường dẫn tới file student.json trong thư mục wwwroot/data
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "student.json");

            // Chuyển đổi danh sách sinh viên thành chuỗi JSON
            string jsonData = JsonSerializer.Serialize(students);

            // Ghi chuỗi JSON vào file student.json
            System.IO.File.WriteAllText(filePath, jsonData);
        }



        public IActionResult Edit(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                // Trả về một form edit trắng nếu không tìm thấy sinh viên với ID được cung cấp
                return View(new Student());
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Kiểm tra trùng lặp email (không bao gồm sinh viên đang được chỉnh sửa)
            if (students.Any(s => s.Email == student.Email && s.Id != student.Id))
            {
                ModelState.AddModelError("Email", "Email already exists.");
            }

            // Kiểm tra trùng lặp số điện thoại (không bao gồm sinh viên đang được chỉnh sửa)
            if (students.Any(s => s.PhoneNumber == student.PhoneNumber && s.Id != student.Id))
            {
                ModelState.AddModelError("PhoneNumber", "Phone number already exists.");
            }

            if (ModelState.IsValid)
            {
                var existingStudent = students.FirstOrDefault(s => s.Id == student.Id);
                if (existingStudent != null)
                {
                    existingStudent.FullName = student.FullName;
                    existingStudent.BirthYear = student.BirthYear;
                    existingStudent.Gender = student.Gender;
                    existingStudent.PhoneNumber = student.PhoneNumber;
                    existingStudent.Email = student.Email;
                    existingStudent.Major = student.Major;

                    // Cập nhật file student.json sau khi chỉnh sửa
                    SaveStudentsToFile();
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }


        public IActionResult Delete(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                // Trả về một view với một đối tượng Student trống nếu không tìm thấy sinh viên với ID được cung cấp
                return View(new Student());
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var student = students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                students.Remove(student);

                // Cập nhật file student.json sau khi xóa
                SaveStudentsToFile();
            }
            return RedirectToAction("Index");
        }
    }
}