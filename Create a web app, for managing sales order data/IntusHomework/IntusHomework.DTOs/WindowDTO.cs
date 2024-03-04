using System.ComponentModel.DataAnnotations;

namespace IntusHomework.DTOs
{
    public class WindowDTO
    {
        public int WindowId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public int QuantityOfWindows { get; set; }
        public int TotalSubElements { get; set; }
        public List<SubElementDTO>? SubElements { get; set; }
        public int OrderId { get; set; }
    }
}
