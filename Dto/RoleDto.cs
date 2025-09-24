using System.ComponentModel.DataAnnotations;

namespace TransportApi.Dto
{
    public class RoleDto
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên quyền phải có từ 3 đến 50 ký tự.")]
        public string name { get; set; }
    }
}
