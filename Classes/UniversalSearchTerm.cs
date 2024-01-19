using System.ComponentModel.DataAnnotations;

namespace PalletSyncApi.Classes
{
    public class UniversalSearchTerm
    {
        [Required]
        [StringLength(450, ErrorMessage = "The search term must be 450 characters or less.")]
        public required string SearchTerm { get; set; }
    }
}
