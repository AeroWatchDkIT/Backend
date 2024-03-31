using PalletSyncApi.Enums;

namespace PalletSyncApi.Classes
{
    public class User
    {
        public string Id { get; set; }
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passcode { get; set; }
        public bool ForkliftCertified { get; set; }
        public int IncorrectPalletPlacements { get; set; }
        public int CorrectPalletPlacements { get; set; }
        public string ImageFilePath { get; set; }
    }
}
