using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace IntAlk1_Assignment.Models
{
    public class OwnerModel
    {
        public int Id { get; set; }
        [DisplayName("Owner's Full Name")]
        public string Name { get; set; }
        public List<PropertyModel> Properties = new();
    }
}
