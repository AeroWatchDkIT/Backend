using PalletSyncApi.Enums;

namespace PalletSyncApi.Classes
{
    public class PalletTrackingLog
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Action { get; set; }
        public string PalletId { get; set; }
        public Pallet Pallet { get; set; }
        public PalletState PalletState { get; set; }
        public string PalletLocation { get; set; }
        public string ForkliftId { get; set; }
        public Forklift Forklift { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }  

    }
}