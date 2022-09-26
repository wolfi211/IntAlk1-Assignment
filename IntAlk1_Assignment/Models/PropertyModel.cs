using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntAlk1_Assignment.Models
{
    public class PropertyModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Address of the Property")]
        public string Address { get; set; }

        [DisplayName("The paid rent for a month")]
        [DataType(DataType.Currency)]
        public int Rent { get; set; }

        public OwnerModel? Owner { get; set; }
        public TenantModel? Tenant { get; set; }

        [DisplayName("Owner:")]
        public int OwnerId { get; set; }
        [DisplayName("Tenant:")]
        public int TenantId { get; set; }
    }
}
