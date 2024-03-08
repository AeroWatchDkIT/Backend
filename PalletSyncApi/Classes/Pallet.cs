using PalletSyncApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace PalletSyncApi.Classes
{
    public class Pallet
    {
        [Required]
        //[Regex(@"^P-[0-9]+$", ErrorMessage = "ID must be in the P-1234 format")]
        [StringLength(450, ErrorMessage = "The id must be 450 characters or less.")]
        public string Id { get; set; } //Pallet code
        public PalletState State { get; set; }
        public string Location { get; set; }
        
        public Pallet()
        {

        }
    }
}
