using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models
{
    public class AuthorDto :BaseDto  
    {
        //ublic int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(250)]
        public string Bio { get; set; }
    }
}
