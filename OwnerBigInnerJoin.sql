select dbo.Owners.Id, dbo.Owners.Name, dbo.Properties.Id as AddressId, dbo.Properties.Address, dbo.Properties.Tenant, dbo.Tenants.Name as TenantName, dbo.Properties.Rent
from ((dbo.Owners
inner join dbo.Properties on dbo.Owners.Id = dbo.Properties.Owner)
inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id)
--where dbo.Owners.Name like '%ay%'
order by dbo.Owners.Id asc