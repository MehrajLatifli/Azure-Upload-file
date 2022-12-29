using Microsoft.AspNetCore.Identity;

namespace Azure_Upload_file_API.IdentityAuth
{
    public class CustomUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
