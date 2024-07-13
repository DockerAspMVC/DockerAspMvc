using System.ComponentModel.DataAnnotations;

namespace DockerMvc.ModelView
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}