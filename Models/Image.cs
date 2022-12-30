namespace ReservationSystem22.Models
{
    public class Image
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public virtual Item? Target { get; set; }

    }

    public class ImageDTO
    {
        public string? Description { get; set; }
        public string? Url { get; set; }
    }
}
