using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SIMS_WEB_APDP.Models;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace SIMS_MVC_SE06204.Controllers
{
    public class CourseController : Controller
    {
        private static List<Course> courses = new List<Course>
        {
            new Course { Id = 1, courseName = "Computer Science", teacherName = "Mr. Hieu" },
            new Course { Id = 2, courseName = "Information Technology", teacherName = "Mrs. Phong" },
            new Course { Id = 3, courseName = "Marketing", teacherName = "Mrs. Quan" }
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
                return RedirectToAction("Login", "Authentication", new { returnUrl = Url.Action("Index", "Course") });
            }

            var filteredCourses = string.IsNullOrEmpty(searchString)
                ? courses
                : courses.Where(c => c.courseName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();

            return View(filteredCourses);
        }

        // Hiển thị form tạo khóa học
        public IActionResult Create()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (ModelState.IsValid)
            {
                course.Id = courses.Any() ? courses.Max(c => c.Id) + 1 : 1;
                courses.Add(course);
                SaveCourseToFile();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        private void SaveCourseToFile()
        {
            // Đường dẫn tới file course.json trong thư mục wwwroot/data
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "course.json");

            // Chuyển đổi danh sách khoa hoc thành chuỗi JSON
            string jsonData = JsonSerializer.Serialize(courses);

            // Ghi chuỗi JSON vào filecourse.json
            System.IO.File.WriteAllText(filePath, jsonData);
        }

        // Hiển thị form chỉnh sửa khóa học
        public IActionResult Edit(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }
            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                // Nếu không tìm thấy khóa học với ID được cung cấp, tạo một đối tượng Course rỗng
                course = new Course { Id = id };
            }
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (ModelState.IsValid)
            {
                var existingCourse = courses.FirstOrDefault(c => c.Id == course.Id);
                if (existingCourse != null)
                {
                    existingCourse.courseName = course.courseName;
                    existingCourse.teacherName = course.teacherName;


                    SaveCourseToFile();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // Hiển thị form xóa khóa học
        public IActionResult Delete(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            SaveCourseToFile();
            return View(course);
        }

        // Xác nhận xóa khóa học
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                courses.Remove(course);
                SaveCourseToFile();
            }
            return RedirectToAction(nameof(Index));
        }

        // Lấy danh sách các khóa học dưới dạng JSON để sử dụng trong các phần khác của ứng dụng
        public JsonResult GetCourses()
        {
            var courseList = courses.Select(c => new
            {
                c.Id,
                c.courseName
            }).ToList();

            return Json(courseList);
        }
    }
}