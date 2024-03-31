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

        //Perhaps we could add a "TotalPalletsHandled" field to get the proportion of incorrectly placed pallets from all pallets handled
    }
}
