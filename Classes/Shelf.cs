using System.ComponentModel.DataAnnotations;

namespace PalletSyncApi.Classes
{
    public class Shelf
    {
        [Required]
        [Regex(@"^S-[0-9]+$", ErrorMessage = "ID must be in the S-1234 format")]
        [StringLength(450, ErrorMessage = "The id must be 450 characters or less.")]
        public string Id { get; set; } //Shelf code
        public string? PalletId { get; set; }
        public Pallet? Pallet { get; set; }
        public string Location { get; set; }
        public Shelf() {
            
        }
    }
}
