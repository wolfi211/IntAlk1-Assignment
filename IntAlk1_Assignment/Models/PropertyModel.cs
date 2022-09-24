using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;

namespace IntAlk1_Assignment.Models
{
    public class PropertyModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public int Rent { get; set; }

        public OwnerModel? Owner { get; set; }
        public TenantModel? Tenant { get; set; }
    }
}
