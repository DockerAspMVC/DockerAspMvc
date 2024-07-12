using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DockerMvc.Models
{
    public class RoleProfile
    {
        [Key]
        public int ProRoleId { get; set; }

        [ForeignKey("Profile")]
        public int ProId { get; set; }
        public Profile Profile { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}