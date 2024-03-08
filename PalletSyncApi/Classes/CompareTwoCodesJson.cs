namespace PalletSyncApi.Classes
{
    public class CompareTwoCodesJson
    {
        public Pallet Pallet { get; set; }
        public Shelf? Shelf { get; set; }
        public DateTime TimeOfInteraction { get; set; }
        public string Action {  get; set; }
        public string UserId { get; set; }
        public string ForkliftId { get; set; }
    }
}
