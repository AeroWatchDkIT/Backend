namespace PalletSyncApi.Classes
{
    public class Forklift
    {
        public string Id { get; set; }
        public string? LastUserId { get; set; }
        public User? LastUser { get; set; }
        public string? LastPalletId { get; set; }
        public Pallet? LastPallet { get; set; }


    }
}
