use IntAlk;

delete from Rents;
delete from Properties;
dbcc checkident ('dbo.Properties', RESEED, 0);
delete from Owners;
dbcc checkident ('dbo.Owners', RESEED, 0);
delete from Tenants;
dbcc checkident ('dbo.Tenants', RESEED, 0);