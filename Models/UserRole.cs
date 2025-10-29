using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Day06_Demo.Models
{
    [PrimaryKey("UserId", "RoleId")]
    public class UserRole
    {
        [ForeignKey("User"), Required]
        public int UserId { get; set; }
        [ForeignKey("Role"), Required]
        public int RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
