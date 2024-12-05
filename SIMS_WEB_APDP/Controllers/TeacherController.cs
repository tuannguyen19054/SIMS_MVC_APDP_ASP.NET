//using Microsoft.AspNetCore.Mvc;
//using SIMS_WEB_APDP.Models;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.AspNetCore.Http;

//namespace SIMS_WEB_APDP.Controllers
//{
//    public class TeacherController : Controller
//    {
//        private static List<Teacher> students = new List<Teacher>
//        {
//            new Teacher { Id = 1, FullName = "Nguyen Van A", BirthYear = new DateTime(2000, 1, 1), Gender = "Male", PhoneNumber = "0123456789", Email = "a@example.com", Major = "Computer Science" },
//            new Teacher { Id = 2, FullName = "Le Thi B", BirthYear = new DateTime(2001, 2, 2), Gender = "Female", PhoneNumber = "0987654321", Email = "b@example.com", Major = "Information Technology" },
//            new Teacher { Id = 3, FullName = "Pham Van C", BirthYear = new DateTime(2004, 2, 2), Gender = "Male", PhoneNumber = "0366144145", Email = "c@example.com", Major = "Marketing" }
//        };

//        // Kiểm tra trạng thái đăng nhập
//        private bool IsUserLoggedIn()
//        {
//            return HttpContext.Session.GetString("UserName") != null;
//        }

//        public IActionResult Index(string searchString)
//        {
//            if (!IsUserLoggedIn())
//            {
//                return RedirectToAction("Login", "Authentication");
//            }

//            var filteredStudents = string.IsNullOrEmpty(searchString)
//                ? students
//                : students.Where(s => s.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
//                                      s.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
//                                      s.PhoneNumber.Contains(searchString)).ToList();

//            ViewData["CurrentFilter"] = searchString; // Lưu từ khóa tìm kiếm vào ViewData
//            return View(filteredStudents);
//        }

//        public IActionResult Create()
//        {
//            if (!IsUserLoggedIn())
//            {
//                return RedirectToAction("Login", "Authentication");
//            }

//            return View();
//        }

//        [HttpPost]
//        public IActionResult Create(Student student)
//        {
//            if (!IsUserLoggedIn())
//            {
//                return RedirectToAction("Login", "Authentication");
//            }

//            if (ModelState.IsValid)
//            {
//                student.Id = students.Count + 1;
//                students.Add(student);
//                return RedirectToAction("Index");
//            }
//            return View(student);
//        }

//        public IActionResult Edit(int id)
//        {
//            if (!IsUserLoggedIn())
//            {
//                return RedirectToAction("Login", "Authentication");
//            }

//            var student = students.FirstOrDefault(s => s.Id == id);
//            if (student == null)
//            {
//                return NotFound();
//            }
//            return View(student);
//        }

//        [HttpPost]
//        public IActionResult Edit(Student student)
//        {
//            if (!IsUserLoggedIn())
//            {
//                return RedirectToAction("Login", "Authentication");
//            }

//            if (ModelState.IsValid)
//            {
//                var existingStudent = students.FirstOrDefault(s => s.Id == student.Id);
//                if (existingStudent != null)
//                {
//                    existingStudent.FullName = student.FullName;
//                    existingStudent.BirthYear = student.BirthYear;
//                    existingStudent.Gender = student.Gender;
//                    existingStudent.PhoneNumber = student.PhoneNumber;
//                    existingStudent.Email = student.Email;
//                    existingStudent.Major = student.Major;
//                }
//                return RedirectToAction("Index");
//            }
//            return View(student);
//        }

//        public IActionResult Delete(int id)
//        {
//            if (!IsUserLoggedIn())
//            {
//                return RedirectToAction("Login", "Authentication");
//            }

//            var student = students.FirstOrDefault(s => s.Id == id);
//            if (student == null)
//            {
//                return NotFound();
//            }
//            return View(student);
//        }

//        [HttpPost]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            if (!IsUserLoggedIn())
//            {
//                return RedirectToAction("Login", "Authentication");
//            }

//            var student = students.FirstOrDefault(s => s.Id == id);
//            if (student != null)
//            {
//                students.Remove(student);
//            }
//            return RedirectToAction("Index");
//        }
//        [HttpPost]
//        public List<Student> GetStudentsByPage(int page, int pageSize)
//        {
//            // Kiểm tra xem page và pageSize có hợp lệ không
//            if (page <= 0 || pageSize <= 0)
//            {
//                throw new ArgumentException("Page and PageSize must be greater than 0.");
//            }

//            // Tính toán vị trí bắt đầu của trang
//            int startIndex = (page - 1) * pageSize;

//            // Nếu startIndex lớn hơn hoặc bằng số lượng khóa học, trả về danh sách rỗng
//            if (startIndex >= students.Count)
//            {
//                return new List<Student>();
//            }

//            // Lấy các khóa học cho trang hiện tại
//            return students.Skip(startIndex).Take(pageSize).ToList();
//        }
//        [HttpPost]
//        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
//        {
//            int totalStudents = students.Count;
//            int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

//            // Kiểm tra nếu pageNumber nằm ngoài phạm vi
//            if (pageNumber < 1) pageNumber = 1;
//            if (pageNumber > totalPages) pageNumber = totalPages;

//            // Lấy danh sách khóa học cho trang hiện tại
//            var paginatedCourses = students
//                                    .Skip((pageNumber - 1) * pageSize)
//                                    .Take(pageSize)
//                                    .ToList();

//            // Truyền thông tin về trang hiện tại và tổng số trang tới View
//            ViewBag.PageNumber = pageNumber;
//            ViewBag.TotalPages = totalPages;

//            return View(paginatedCourses);
//        }
//    }
//}
