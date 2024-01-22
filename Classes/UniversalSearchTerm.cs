using System.ComponentModel.DataAnnotations;

namespace PalletSyncApi.Classes
{
<<<<<<<< HEAD:Classes/SearchQuery.cs
    public class SearchQuery
========
    public class UniversalSearchTerm
>>>>>>>> CabinViewEndpoints:Classes/UniversalSearchTerm.cs
    {
        [Required]
        [StringLength(450, ErrorMessage = "The search term must be 450 characters or less.")]
        public required string SearchTerm { get; set; }
    }
}
