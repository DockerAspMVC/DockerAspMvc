using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DockerMvc.Models
{
    public class SubCategoria
    {
        [Key]
        public int SubId { get; set; }
        
        [Display(Name = "Imagen de SubCategoría")]
        public string Image { get; set; }
        
        [ForeignKey("Productos")]
        public int ProduId { get; set; }
        public Productos Productos { get; set; }

        [ForeignKey("Categoria")]
        public int CatId { get; set; }
        public Categoria Categoria { get; set; }
       
    }
}