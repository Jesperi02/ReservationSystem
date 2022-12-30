using System.ComponentModel.DataAnnotations;

namespace ReservationSystem22.Models
{
    public class Item
    {
        public long Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} length must be between {2} and {1}")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual User? Owner { get; set; }
        public virtual List<Image>? Images { get; set; }
        public long accessCount { get; set; }
    }

    public class ItemDTO
    {
        public long Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} length must be between {2} and {1}")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Owner { get; set; }
        public virtual List<ImageDTO>? Images { get; set; }
    }
}
