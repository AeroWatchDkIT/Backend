namespace PalletSyncApi.Classes
{
    public class ErrorResponse
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
