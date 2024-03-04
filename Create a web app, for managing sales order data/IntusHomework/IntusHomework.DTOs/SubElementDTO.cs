using System.ComponentModel.DataAnnotations;

namespace IntusHomework.DTOs
{
    public class SubElementDTO
    {
        public int SubElementId { get; set; }
        public int Element { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int WindowId { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        public string Type { get; set; }
    }
}
