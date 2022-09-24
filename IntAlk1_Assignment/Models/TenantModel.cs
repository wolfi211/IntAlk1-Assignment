namespace IntAlk1_Assignment.Models
{
    public class TenantModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PropertyModel> Properties = new();
    }
}
