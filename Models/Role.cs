using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Day06_Demo.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        [Remote("RoleExist", "Role", AdditionalFields = "Id", ErrorMessage = "This Role Exist In DB!")]
        public string RoleName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
