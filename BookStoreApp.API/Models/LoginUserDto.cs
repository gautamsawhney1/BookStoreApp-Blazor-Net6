using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models
{
    public class LoginUserDto
    {
        // public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
       
       
    }

}