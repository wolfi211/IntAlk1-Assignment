--SELECT dbo.Properties.Id, dbo.Properties.Address, dbo.Properties.Owner, dbo.Owners.Name as OwnerName, dbo.Properties.Tenant, dbo.Tenants.Name as TenantName, dbo.Properties.Rent
--FROM ((dbo.Properties
--INNER JOIN dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id)
--inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id);

select dbo.Properties.Id, dbo.Properties.Address, dbo.Properties.Owner as OwnerId, 
dbo.Owners.Name as OwnerName, dbo.Properties.Tenant TenantId, dbo.Tenants.Name as TenantName, dbo.Properties.Rent 
from ((dbo.Properties
inner join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id)
inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id)