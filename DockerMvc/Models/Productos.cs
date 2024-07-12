using System.ComponentModel.DataAnnotations;

namespace DockerMvc.Models
{
    public class Productos
    {
        [Key]
        public int ProduId { get; set; }
        
        [Display(Name = "Nombre")]
        [Required]
        public string Nombre { get; set; }
        
        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        
        [Display(Name = "Imagen")]
        public string Imagen { get; set; }
        
        [Display(Name = "Precio")]
        [Required]
        public float Precio { get; set; }
        
        [Display(Name = "Stock")]
        [Required]
        public int Stock { get; set; }
    }
}