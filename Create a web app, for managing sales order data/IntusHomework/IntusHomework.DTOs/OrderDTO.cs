using System.ComponentModel.DataAnnotations;

namespace IntusHomework.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(2, ErrorMessage = "State must be a two-letter abbreviation.")]
        public string State { get; set; }

        public List<WindowDTO>? Windows { get; set; }
    }
}
