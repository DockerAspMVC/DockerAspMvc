using System.ComponentModel.DataAnnotations;

namespace DockerMvc.Models
{
    public class Categoria
    {
        [Key]
        public int CatId { get; set; }
        
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        
        [Display(Name = "Category Description")]
        public string Description { get; set; }

    }
}