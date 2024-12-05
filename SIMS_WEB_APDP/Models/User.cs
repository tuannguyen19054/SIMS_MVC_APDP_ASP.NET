namespace SIMS_WEB_APDP.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }

        public int FailedLoginAttempts { get; set; } = 0; // Số lần đăng nhập thất bại
        public bool IsLocked { get; set; } = false;
        // Constructor không tham số
        public User() { }

        // Constructor có tham số
        public User(int id, string userName, string passWord, string email, string phoneNumber, string role)
        {
            Id = id;
            UserName = userName;
            PassWord = passWord;
            Email = email;
            PhoneNumber = phoneNumber;
            Role = role;
        }
    }
}
