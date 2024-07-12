using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WestoverLaneReserve.Models
{
    public class CustomerApplicationUser : IdentityUser
    {
        // [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // public int CustomerId { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        // [Required, EmailAddress]
        // public string Email { get; set; } = string.Empty;
        // [Required]
        // [DataType(DataType.Password)]
        // public string Password { get; set; } = string.Empty;

    }
}