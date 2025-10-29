using System.ComponentModel.DataAnnotations;

namespace Day06_Demo.Models
{
    public class LoginVM
    {
        [Required, StringLength(20, MinimumLength = 3)]
        public string UserName { get; set; }
        [EmailAddress,Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
