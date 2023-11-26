namespace PalletSyncApi.Classes
{
    public class Shelf
    {
        public string Id { get; set; } //Shelf code
        public string? PalletId { get; set; }
        public Pallet? Pallet { get; set; }
        public string Location { get; set; }
        public Shelf() {
            
        }
    }
}
