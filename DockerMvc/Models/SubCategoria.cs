using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DockerMvc.Models
{
    public class SubCategoria
    {
        [Key]
        public int SubId { get; set; }

        [Display(Name = "Imagen de SubCategoría")]
        public string Image { get; set; }

        [ForeignKey("Categoria")]
        public int CatId { get; set; }
        public Categoria Categoria { get; set; }

        public ICollection<SubCategoriaProducto> SubCategoriaProductos { get; set; }
    }
    public class SubCategoriaProducto
    {
        [Key]
        public int SubCategoriaProductoId { get; set; }
        
        public int SubCategoriaSubId { get; set; }
        [ForeignKey("SubCategoriaSubId")]
        public SubCategoria SubCategoria { get; set; }
        
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Productos Productos { get; set; }
    }}
