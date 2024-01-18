using System.ComponentModel.DataAnnotations;

namespace PalletSyncApi.Classes
{
    public class PalletStatus
    {
        [Required]
        [Regex(@"^P-[0-9]+$", ErrorMessage = "ID must be in the P-1234 format")]
        [StringLength(450, ErrorMessage = "The id must be 450 characters or less.")]
        public string Name { get; set; }
        public string Place { get; set; }
    }
}
