using System.ComponentModel.DataAnnotations;

namespace DockerMvc.Models
{
    public class Permission
    {
        [Key]
        public int PermId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PermName { get; set; }
    }
}