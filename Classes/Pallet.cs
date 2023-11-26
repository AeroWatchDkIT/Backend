using PalletSyncApi.Enums;

namespace PalletSyncApi.Classes
{
    public class Pallet
    {
        public string Id { get; set; } //Pallet code
        public PalletState State { get; set; }
        public string Location { get; set; }

        public Pallet()
        {
            
        }
    }
}
