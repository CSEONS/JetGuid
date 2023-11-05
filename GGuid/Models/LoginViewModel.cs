using System.ComponentModel;
using System.Security.Permissions;

namespace GGuid.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public bool Persistent { get; set; }
        public string? ReturnUrl { get; set; } = string.Empty;
    }
}
