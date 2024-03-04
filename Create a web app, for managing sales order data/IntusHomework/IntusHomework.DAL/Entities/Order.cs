using System.ComponentModel.DataAnnotations;

namespace IntusHomework.DAL.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public virtual List<Window>? Windows { get; set; } = [];
    }
}