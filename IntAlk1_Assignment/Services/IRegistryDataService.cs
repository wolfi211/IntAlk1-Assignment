using IntAlk1_Assignment.Models;

namespace IntAlk1_Assignment.Services
{
    public interface IRegistryDataService
    {
        List<PropertyModel> GetProperties();
        List<OwnerModel> GetOwners();
        List<TenantModel> GetTenants();

        OwnerModel? GetOwnerById(int Id);
        TenantModel? GetTenantById(int Id);
        PropertyModel? GetPropertyById(int Id);

        List<OwnerModel> SearchOwners(string searchTerm);
        List<TenantModel> SearchTenants(string searchTerm);
        List<PropertyModel> SearchProperties(string searchTerm);

        int InsertOwner(OwnerModel item);
        int InsertTenant(TenantModel item);
        int InsertProperty(PropertyModel item);

        int UpdateOwner(OwnerModel item);
        int UpdateTenant(TenantModel item);
        int UpdateProperty(PropertyModel item);

        int DeleteOwner(int Id);
        int DeleteTenant(int Id);
        int DeleteProperty(int Id);
    }
}
