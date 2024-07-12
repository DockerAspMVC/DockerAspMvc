using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DockerMvc.ModelView
{
    public class ProductosViewModel
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
        [Required]
        public IFormFile Imagen { get; set; }
        
        [Display(Name = "Precio")]
        [Required]
        public float Precio { get; set; }
        
        [Display(Name = "Stock")]
        [Required]
        public int Stock { get; set; }
    }
}