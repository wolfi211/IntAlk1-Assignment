namespace IntAlk1_Assignment.Models
{
    public class RentModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int PropertyId { get; set; }
        public int TenantId { get; set; }
        public int Rent { get; set; }
        public int Payed { get; set; }
        public bool IsPayed { get; set; }
        public string PropertyAddress { get; set; }
        public string TenantName { get; set; }
    }
}
