using System.ComponentModel.DataAnnotations;

namespace IntusHomework.DAL.Entities
{
    public class SubElement
    {
        [Key]
        public int SubElementId { get; set; }

        [Required]
        public int Element { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int WindowId { get; set; }

        public virtual Window Window { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}