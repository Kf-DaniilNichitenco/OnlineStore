using System.ComponentModel.DataAnnotations;

namespace Auth.Quickstart.Account
{
    public class SignupInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        [Required]
        public string RepeatPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
