namespace PalletSyncApi.Classes
{
    public class ForkliftUiResult
    {
        public string Id { get; set; }
        public string? LastUser { get; set; }
        public string? LastPalletId { get; set; }

        public ForkliftUiResult(string id, string lastUser, string lastPalletId)
        {
            Id = id;
            LastUser = lastUser;
            LastPalletId = lastPalletId;
        }
    }
}
