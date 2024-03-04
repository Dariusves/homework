using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntusHomework.DAL.Entities
{
    public class Window
    {
        [Key]
        public int WindowId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int QuantityOfWindows { get; set; }

        [Required]
        public int TotalSubElements { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        public ICollection<SubElement>? SubElements { get; set; } = new HashSet<SubElement>();
    }
}