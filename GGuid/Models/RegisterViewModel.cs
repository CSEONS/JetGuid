using System.Security.Permissions;

namespace GGuid.Models
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepitedPassword { get; set; }
        public string? ReturnUrl { get; set; } = string.Empty;
    }
}
