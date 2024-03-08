using System.ComponentModel.DataAnnotations;

namespace PalletSyncApi.Classes
{
    public class Forklift
    {
        [Required]
        [Regex(@"^F-[0-9]+$", ErrorMessage = "ID must be in the F-1234 format")]
        [StringLength(450, ErrorMessage = "The id must be 450 characters or less.")]
        public string Id { get; set; }
        public string? LastUserId { get; set; }
        public User? LastUser { get; set; }
        public string? LastPalletId { get; set; }
        public Pallet? LastPallet { get; set; }
    }
}
