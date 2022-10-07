using System.ComponentModel;

namespace IntAlk1_Assignment.Models
{
    public class RentModel
    {
        [DisplayName("Year")]
        public int Year { get; set; }
        [DisplayName("Month")]
        public int Month { get; set; }
        public int PropertyId { get; set; }
        public int TenantId { get; set; }
        [DisplayName("Rent ($)")]
        public int Owed { get; set; }
        [DisplayName("Payed ($)")]
        public int Payed { get; set; }
        public bool IsPayed { get; set; }
        [DisplayName("Address")]
        public string PropertyAddress { get; set; }
        [DisplayName("Tenant")]
        public string TenantName { get; set; }
    }
}
