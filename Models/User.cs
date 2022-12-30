using System.ComponentModel.DataAnnotations;

namespace ReservationSystem22.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} length must be between {2} and {1}")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} length must be between {2} and {1}")]
        public string? Password { get; set; }
        public byte[]? Salt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastLoginDate { get; set; }

    }

    public class UserDTO
    {
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
