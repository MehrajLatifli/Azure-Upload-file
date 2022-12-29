using Azure_Upload_file_API.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Azure_Upload_file_API.Services
{
    public class UploadFile
    {
        [Required]
        [DisplayName("UploadFile")]
        [FileSize(555555555, 55555)]
        [AllowedExtensions(new string[] { ".mp4", ".webm", ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".json" })]
        public IFormFile files { get; set; }
    }
}
