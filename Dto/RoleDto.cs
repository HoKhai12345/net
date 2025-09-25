using System.ComponentModel.DataAnnotations;

namespace TransportApi.Dto
{
    public class RoleDto
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên quyền phải có từ 3 đến 50 ký tự.")]
        public string name { get; set; }

        public override string ToString()
        {
            // Sử dụng chuỗi nội suy để trả về thông tin quan trọng nhất
            return $"RoleDto Name={name}]";

            // Hoặc nếu muốn chi tiết hơn:
            // return $"RoleDto [Id={Id}, Name={Name}, Description={Description}]";
        }
    }
}
