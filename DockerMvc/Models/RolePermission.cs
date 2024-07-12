using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DockerMvc.Models
{
    public class RolePermission
    {
        [Key]
        public int RolePermId { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey("Permission")]
        public int PermId { get; set; }
        public Permission Permission { get; set; }
    }
}