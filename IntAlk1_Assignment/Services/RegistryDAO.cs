using IntAlk1_Assignment.Models;
using System.Data.SqlClient;

namespace IntAlk1_Assignment.Services
{
    public class RegistryDAO : IRegistryDataService
    {
        private readonly string connectionString = "Data Source=Localhost;Initial Catalog=IntAlk;Integrated Security=True";

        /*********************************************************************
         * Get EVERYthing from a table
         *********************************************************************/

        #region GET

        public List<OwnerModel> GetOwners()
        {
            List<OwnerModel> owners = new();

            string sqlStatement = "select dbo.Owners.Id, dbo.Owners.Name, dbo.Properties.Id as AddressId, dbo.Properties.Address, " +
                                  "dbo.Properties.Tenant as TenantId, dbo.Tenants.Name as TenantName, dbo.Properties.Rent " +
                                  "from ((dbo.Owners " +
                                  "inner join dbo.Properties on dbo.Owners.Id = dbo.Properties.Owner) " +
                                  "inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id) " +
                                  "order by dbo.Owners.Id asc";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0; //there is no 0 Id in the DB so this should be ok

                    while (reader.Read())
                    {
                        if ((int)reader["Id"] != curId)
                        {
                            curId = (int)reader["Id"];
                            owners.Add(new OwnerModel { Id = (int)reader["Id"], Name = (string)reader["Name"] });
                        }
                        owners[^1].Properties.Add(new PropertyModel
                        {
                            Id = (int)reader["AddressId"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["TenantId"],
                                Name = (string)reader["TenantName"]
                            },
                            Rent = (int)reader["Rent"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return owners;
        }

        public List<TenantModel> GetTenants()
        {
            List<TenantModel> tenants = new();

            string sqlStatement = "select dbo.Tenants.Id, dbo.Tenants.Name, dbo.Properties.Id as AddressId, dbo.Properties.Address, " +
                                  "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, dbo.Properties.Rent " +
                                  "from ((dbo.Tenants " +
                                  "inner join dbo.Properties on dbo.Tenants.Id = dbo.Properties.Tenant) " +
                                  "inner join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                                  "order by dbo.Tenants.Id asc";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0; //there is no 0 Id in the DB so this should be ok

                    while (reader.Read())
                    {
                        if ((int)reader["Id"] != curId)
                        {
                            curId = (int)reader["Id"];
                            tenants.Add(new TenantModel { Id = (int)reader["Id"], Name = (string)reader["Name"] });
                        }
                        tenants[^1].Properties.Add(new PropertyModel
                        {
                            Id = (int)reader["AddressId"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            },
                            Rent = (int)reader["Rent"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return tenants;
        }

        public List<PropertyModel> GetProperties()
        {
            List<PropertyModel> properties = new();

            string sqlStatement = "select dbo.Properties.Id, dbo.Properties.Address, dbo.Properties.Owner as OwnerId, " +
                                  "dbo.Owners.Name as OwnerName, dbo.Properties.Tenant TenantId, dbo.Tenants.Name as TenantName, dbo.Properties.Rent " +
                                  "from ((dbo.Properties " +
                                  "inner join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                                  "inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id)";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        properties.Add(new PropertyModel
                        {
                            Id = (int)reader["Id"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["TenantId"],
                                Name = (string)reader["TenantName"]
                            },
                            Rent = (int)reader["Rent"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return properties;
        }

        public List<RentModel> GetRents()
        {
            var rents = new List<RentModel>();

            string sqlStatement = "select dbo.Rents.Year, dbo.Rents.Month, dbo.Rents.Property as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Rents.Tenant as TenantId, dbo.Tenants.Name as TenantName, dbo.Rents.Rent, dbo.Rents.Payed, dbo.Rents.Done " +
                "from ((dbo.Rents " +
                "inner join dbo.Properties on dbo.Rents.Property = dbo.Properties.Id) " +
                "inner join dbo.Tenants on dbo.Rents.Tenant = dbo.Tenants.Id) " +
                "where dbo.Rents.Done = 0";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        rents.Add(new RentModel
                        {
                            Year = (int)reader["Year"],
                            Month = (int)reader["Month"],
                            PropertyId = (int)reader["PropertyId"],
                            PropertyAddress = (string)reader["PropertyAddress"],
                            TenantId = (int)reader["TenantId"],
                            TenantName = (string)reader["TenantName"],
                            Rent = (int)reader["Rent"],
                            Payed = (int)reader["Payed"],
                            IsPayed = (bool)reader["Done"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return rents;
        }

        #endregion

        /*********************************************************************
         * Get Things By ID
         *********************************************************************/

        #region GET_BY_ID

        public OwnerModel? GetOwnerById(int Id)
        {
            OwnerModel owner = null;

            string sqlStatement = "select dbo.Owners.Id, dbo.Owners.Name, dbo.Properties.Id as AddressId, dbo.Properties.Address, " +
                                  "dbo.Properties.Tenant as TenantId, dbo.Tenants.Name as TenantName, dbo.Properties.Rent " +
                                  "from ((dbo.Owners " +
                                  "inner join dbo.Properties on dbo.Owners.Id = dbo.Properties.Owner) " +
                                  "inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id) " +
                                  "where dbo.Owners.Id = @OwnerId " +
                                  "order by dbo.Owners.Id asc";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);
                cmd.Parameters.AddWithValue("@OwnerId", Id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0; //there is no 0 Id in the DB so this should be ok

                    while (reader.Read())
                    {
                        if ((int)reader["Id"] != curId)
                        {
                            curId = (int)reader["Id"];
                            owner = new OwnerModel { Id = (int)reader["Id"], Name = (string)reader["Name"] };
                        }
                        owner.Properties.Add(new PropertyModel
                        {
                            Id = (int)reader["AddressId"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["TenantId"],
                                Name = (string)reader["TenantName"]
                            },
                            Rent = (int)reader["Rent"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return owner;
        }

        public TenantModel? GetTenantById(int Id)
        {
            TenantModel tenant = null;

            string sqlStatement = "select dbo.Tenants.Id, dbo.Tenants.Name, dbo.Properties.Id as AddressId, dbo.Properties.Address, " +
                                  "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, dbo.Properties.Rent " +
                                  "from ((dbo.Tenants " +
                                  "inner join dbo.Properties on dbo.Tenants.Id = dbo.Properties.Tenant) " +
                                  "inner join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                                  "where dbo.Tenants.Id = @TenantId " +
                                  "order by dbo.Tenants.Id asc";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);
                cmd.Parameters.AddWithValue("@TenantId", Id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0; //there is no 0 Id in the DB so this should be ok

                    while (reader.Read())
                    {
                        if ((int)reader["Id"] != curId)
                        {
                            curId = (int)reader["Id"];
                            tenant = new TenantModel { Id = (int)reader["Id"], Name = (string)reader["Name"] };
                        }
                        tenant.Properties.Add(new PropertyModel
                        {
                            Id = (int)reader["AddressId"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            },
                            Rent = (int)reader["Rent"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return tenant;
        }

        public PropertyModel? GetPropertyById(int Id)
        {
            PropertyModel property = null;

            string sqlStatement = "select dbo.Properties.Id, dbo.Properties.Address, dbo.Properties.Owner as OwnerId, " +
                                  "dbo.Owners.Name as OwnerName, dbo.Properties.Tenant TenantId, dbo.Tenants.Name as TenantName, dbo.Properties.Rent " +
                                  "from ((dbo.Properties " +
                                  "inner join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                                  "inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id) " +
                                  "where dbo.Properties.Id = @PropertyId;";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@PropertyId", Id);

                try
                {
                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        property = new PropertyModel
                        {
                            Id = (int)reader["Id"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["TenantId"],
                                Name = (string)reader["TenantName"]
                            },
                            Rent = (int)reader["Rent"]
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return property;
        }

        #endregion

        /*********************************************************************
         * Search For Things
         *********************************************************************/

        #region SEARCH

        public List<OwnerModel> SearchOwners(string searchTerm)
        {
            List<OwnerModel> owners = new();

            string sqlStatement = "select dbo.Owners.Id, dbo.Owners.Name, dbo.Properties.Id as AddressId, dbo.Properties.Address, " +
                                  "dbo.Properties.Tenant as TenantId, dbo.Tenants.Name as TenantName, dbo.Properties.Rent " +
                                  "from ((dbo.Owners " +
                                  "inner join dbo.Properties on dbo.Owners.Id = dbo.Properties.Owner) " +
                                  "inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id) " +
                                  "where dbo.Owners.Name like @SearchTerm " +
                                  "order by dbo.Owners.Id asc";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);
                cmd.Parameters.AddWithValue("@SearchTerm", '%' + searchTerm + '%');

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0; //there is no 0 Id in the DB so this should be ok

                    while (reader.Read())
                    {
                        if ((int)reader["Id"] != curId)
                        {
                            curId = (int)reader["Id"];
                            owners.Add(new OwnerModel { Id = (int)reader["Id"], Name = (string)reader["Name"] });
                        }
                        owners[^1].Properties.Add(new PropertyModel
                        {
                            Id = (int)reader["AddressId"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["TenantId"],
                                Name = (string)reader["TenantName"]
                            },
                            Rent = (int)reader["Rent"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return owners;
        }

        public List<TenantModel> SearchTenants(string searchTerm)
        {
            List<TenantModel> tenants = new();

            string sqlStatement = "select dbo.Tenants.Id, dbo.Tenants.Name, dbo.Properties.Id as AddressId, dbo.Properties.Address, " +
                                  "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, dbo.Properties.Rent " +
                                  "from ((dbo.Tenants " +
                                  "inner join dbo.Properties on dbo.Tenants.Id = dbo.Properties.Tenant) " +
                                  "inner join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                                  "where dbo.Tenant.Name like @SearchTerm " +
                                  "order by dbo.Tenants.Id asc";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);
                cmd.Parameters.AddWithValue("@SearchTerm", '%' + searchTerm + '%');

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0; //there is no 0 Id in the DB so this should be ok

                    while (reader.Read())
                    {
                        if ((int)reader["Id"] != curId)
                        {
                            curId = (int)reader["Id"];
                            tenants.Add(new TenantModel { Id = (int)reader["Id"], Name = (string)reader["Name"] });
                        }
                        tenants[^1].Properties.Add(new PropertyModel
                        {
                            Id = (int)reader["AddressId"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            },
                            Rent = (int)reader["Rent"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return tenants;
        }

        public List<PropertyModel> SearchProperties(string searchTerm)
        {
            List<PropertyModel> properties = new();

            string sqlStatement = "select dbo.Properties.Id, dbo.Properties.Address, dbo.Properties.Owner as OwnerId, " +
                                  "dbo.Owners.Name as OwnerName, dbo.Properties.Tenant TenantId, dbo.Tenants.Name as TenantName, dbo.Properties.Rent " +
                                  "from ((dbo.Properties " +
                                  "inner join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                                  "inner join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id) " +
                                  "where dbo.Properties.Address like @SearchTerm";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@SearchTerm", '%' + searchTerm + '%');

                try
                {
                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        properties.Add(new PropertyModel
                        {
                            Id = (int)reader["Id"],
                            Address = (string)reader["Address"],
                            Owner = new OwnerModel
                            {
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            },
                            Tenant = new TenantModel
                            {
                                Id = (int)reader["TenantId"],
                                Name = (string)reader["TenantName"]
                            },
                            Rent = (int)reader["Rent"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return properties;
        }

        #endregion

        /*********************************************************************
         * Insert new things into DB
         *********************************************************************/

        #region INSERT

        public int InsertOwner(OwnerModel owner)
        {
            string sqlStatement = "INSERT INTO dbo.Owners (Name) VALUES (@Name)";
            int newId = -1;

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Name", owner.Name);

                try
                {
                    connection.Open();

                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return newId;
        }

        public int InsertTenant(TenantModel tenant)
        {
            string sqlStatement = "INSERT INTO dbo.Tenants (Name) VALUES (@Name)";
            int newId = -1;

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Name", tenant.Name);

                try
                {
                    connection.Open();

                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return newId;
        }

        public int InsertProperty(PropertyModel property)
        {
            string sqlStatement = "INSERT INTO dbo.Properties (Address, Owner, Tenant, Rent) VALUES (@Address, @Owner, @Tenant, @Rent)";
            int newId = -1;

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Address", property.Address);
                cmd.Parameters.AddWithValue("@Owner", property.OwnerId);
                cmd.Parameters.AddWithValue("@Tenant", property.TenantId);
                cmd.Parameters.AddWithValue("@Rent", property.Rent);

                try
                {
                    connection.Open();

                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return newId;
        }

        #endregion

        /*********************************************************************
         * Modify already existing things
         *********************************************************************/

        #region UPDATE

        public int UpdateOwner(OwnerModel owner)
        {
            int reply = -1;
            string sqlStatement = "UPDATE dbo.Owners SET Name = @Name WHERE Id = @Id";

            using (SqlConnection connection = new (connectionString))
            {
                SqlCommand cmd = new (sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Name", owner.Name);
                cmd.Parameters.AddWithValue("@Id", owner.Id);
                Console.WriteLine(cmd.ToString());

                try
                {
                    connection.Open();
                    reply = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reply;
        }

        public int UpdateTenant(TenantModel tenant)
        {
            int reply = -1;
            string sqlStatement = "UPDATE dbo.Tenants SET Name=@Name WHERE Id=@Id";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Name", tenant.Name);
                cmd.Parameters.AddWithValue("@Id", tenant.Id);

                try
                {
                    connection.Open();
                    reply = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reply;
        }

        public int UpdateProperty(PropertyModel property)
        {
            int reply = -1;
            string sqlStatement = "UPDATE dbo.Properties SET Address=@Address, Owner=@Owner, Tenant=@Tenant, Rent=@Rent WHERE Id=@Id";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Address", property.Address);
                cmd.Parameters.AddWithValue("@Owner", property.OwnerId);
                cmd.Parameters.AddWithValue("@Tenant", property.TenantId);
                cmd.Parameters.AddWithValue("@Rent", property.Rent);
                cmd.Parameters.AddWithValue("@Id", property.Id);

                try
                {
                    connection.Open();
                    reply = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reply;
        }

        #endregion

        /*********************************************************************
         * Delete things
         *********************************************************************/

        #region DELETE

        public int DeleteOwner(int Id)
        {
            int reply = -1;
            string sqlStatement = "DELETE FROM dbo.Owners WHERE Id = @Id";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Id", Id);

                try
                {
                    connection.Open();

                    reply = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reply;
        }

        public int DeleteTenant(int Id)
        {
            int reply = -1;
            string sqlStatement = "DELETE FROM dbo.Tenants WHERE Id = @Id";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Id", Id);

                try
                {
                    connection.Open();

                    reply = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reply;
        }

        public int DeleteProperty(int Id)
        {
            int reply = -1;
            string sqlStatement = "DELETE FROM dbo.Properties WHERE Id = @Id";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Id", Id);

                try
                {
                    connection.Open();

                    reply = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reply;
        }

        #endregion
    }
}
