using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DockerMvc.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}