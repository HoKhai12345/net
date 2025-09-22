using System.ComponentModel.DataAnnotations;

namespace TransportApi.Models
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Tên là bắt buộc")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [MinLength(3, ErrorMessage = "Tên đăng nhập phải có ít nhất 3 ký tự")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string? Password { get; set; }
    }
}
