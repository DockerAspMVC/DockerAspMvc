﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DockerMvc.Models
{
    public class Profile
    {
        [Key]
        public int ProId { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [MaxLength(100)]
        public string ProName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Apellido")]
        public string ProLastname { get; set; }

        [Required]
        [Display(Name = "Correo")]
        public string ProEmail { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        public string ProPassword { get; set; }

        public byte[] ProEncryptedPass { get; set; }

        [MaxLength(1)]
        public char ProEstado { get; set; } = 'A';

        [Required]
        [Display(Name = "Foto de Perfil")]
        public string ProImage { get; set; }
    }
}