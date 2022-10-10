using IntAlk1_Assignment.Models;
using System.Data.SqlClient;

namespace IntAlk1_Assignment.Services
{
    public class RegistryDAO : IRegistryDataService
    {
        private readonly string connectionString = "Data Source=Localhost;Initial Catalog=IntAlk;Integrated Security=True";
        private Logger _logger = new();

        /*********************************************************************
         * Get EVERYthing from a table
         *********************************************************************/

        #region GET

        public List<OwnerModel> GetOwners()
        {
            List<OwnerModel> owners = new();

            string sqlStatement = 
                "select dbo.Owners.Id as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Tenant as TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Rent " +
                "from ((dbo.Owners " +
                "left join dbo.Properties on dbo.Owners.Id = dbo.Properties.Owner) " +
                "left join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id) " +
                "order by OwnerName asc";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0;

                    while (reader.Read())
                    {
                        if ((int)reader["OwnerId"] != curId)
                        {
                            curId = (int)reader["OwnerId"];
                            owners.Add(
                                new OwnerModel { 
                                Id = (int)reader["OwnerId"], 
                                Name = (string)reader["OwnerName"] 
                            });
                        }
                        if (!DBNull.Value.Equals(reader["PropertyId"]))
                        {
                            owners[^1].Properties.Add(new PropertyModel
                            {
                                Id = (int)reader["PropertyId"],
                                Address = (string)reader["PropertyAddress"],
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

            string sqlStatement = 
                "select dbo.Tenants.Id as TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Rent " +
                "from ((dbo.Tenants " +
                "left join dbo.Properties on dbo.Tenants.Id = dbo.Properties.Tenant) " +
                "left join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                "order by TenantName asc";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0;

                    while (reader.Read())
                    {
                        if ((int)reader["TenantId"] != curId)
                        {
                            curId = (int)reader["TenantId"];
                            tenants.Add(
                                new TenantModel { 
                                    Id = (int)reader["TenantId"], 
                                    Name = (string)reader["TenantName"] 
                                });
                        }
                        if (!DBNull.Value.Equals(reader["PropertyId"]))
                        {
                            tenants[^1].Properties.Add(new PropertyModel
                            {
                                Id = (int)reader["PropertyId"],
                                Address = (string)reader["PropertyAddress"],
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

            string sqlStatement = 
                "select dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Tenant TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Rent " +
                "from ((dbo.Properties " +
                "left join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                "left join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id) " +
                "order by PropertyAddress asc";

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
                            Id = (int)reader["PropertyId"],
                            Address = (string)reader["PropertyAddress"],
                            Rent = (int)reader["Rent"]
                        });
                        if (!DBNull.Value.Equals(reader["OwnerId"]))
                        {
                            properties[^1].Owner = new OwnerModel
                            {
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            };
                            properties[^1].OwnerId = (int)reader["OwnerId"];
                        }
                        if (!DBNull.Value.Equals(reader["TenantId"]))
                        {
                            properties[^1].Tenant = new TenantModel
                            {
                                Id = (int)reader["TenantId"],
                                Name = (string)reader["TenantName"]
                            };
                            properties[^1].TenantId = (int)reader["TenantId"];
                        }
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

            string sqlStatement = 
                "select dbo.Rents.Year, dbo.Rents.Month, " +
                "dbo.Rents.Property as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Rents.Tenant as TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Rents.Owed, dbo.Rents.Payed, dbo.Rents.isPayed " +
                "from ((dbo.Rents " +
                "inner join dbo.Properties on dbo.Rents.Property = dbo.Properties.Id) " +
                "inner join dbo.Tenants on dbo.Rents.Tenant = dbo.Tenants.Id) " +
                "where dbo.Rents.isPayed = 0";

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
                            Owed = (int)reader["Owed"],
                            Payed = (int)reader["Payed"],
                            IsPayed = (bool)reader["isPayed"]
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

            string sqlStatement = 
                "select dbo.Owners.Id as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Tenant as TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Rent " +
                "from ((dbo.Owners " +
                "left join dbo.Properties on dbo.Owners.Id = dbo.Properties.Owner) " +
                "left join dbo.Tenants on dbo.Properties.Tenant = dbo.Tenants.Id) " +
                "where dbo.Owners.Id = @OwnerId ";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);
                cmd.Parameters.AddWithValue("@OwnerId", Id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0;

                    //[while] neccesary, there will be more data lines if owner has more properties
                    while (reader.Read())
                    {
                        if ((int)reader["OwnerId"] != curId)
                        {
                            curId = (int)reader["OwnerId"];
                            owner = new OwnerModel { 
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            };
                        }
                        if (!DBNull.Value.Equals(reader["PropertyId"]))
                        {
                            owner.Properties.Add(new PropertyModel
                            {
                                Id = (int)reader["PropertyId"],
                                Address = (string)reader["PropertyAddress"],
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

            string sqlStatement = 
                "select dbo.Tenants.Id as TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Rent " +
                "from ((dbo.Tenants " +
                "left join dbo.Properties on dbo.Tenants.Id = dbo.Properties.Tenant) " +
                "left join dbo.Owners on dbo.Properties.Owner = dbo.Owners.Id) " +
                "where dbo.Tenants.Id = @TenantId ";

            using (SqlConnection conn = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, conn);
                cmd.Parameters.AddWithValue("@TenantId", Id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int curId = 0;

                    //[while] neccesary, there will be more data lines if owner has more properties
                    while (reader.Read())
                    {
                        if ((int)reader["TenantId"] != curId)
                        {
                            curId = (int)reader["TenantId"];
                            tenant = new TenantModel { 
                                Id = (int)reader["TenantId"], 
                                Name = (string)reader["TenantName"]
                            };
                        }
                        if (!DBNull.Value.Equals(reader["PropertyId"]))
                        {
                            tenant.Properties.Add(new PropertyModel
                            {
                                Id = (int)reader["PropertyId"],
                                Address = (string)reader["PropertyAddress"],
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

            string sqlStatement = 
                "select dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Tenant TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Rent " +
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
                            Id = (int)reader["PropertyId"],
                            Address = (string)reader["PropertyAddress"],
                            Rent = (int)reader["Rent"]
                        };
                        if (!DBNull.Value.Equals(reader["OwnerId"]))
                        {
                            property.Owner = new OwnerModel
                            {
                                Id = (int)reader["OwnerId"],
                                Name = (string)reader["OwnerName"]
                            };
                            property.OwnerId = (int)reader["OwnerId"];
                        }
                        if (!DBNull.Value.Equals(reader["TenantId"]))
                        {
                            property.Tenant = new TenantModel
                            {
                                Id = (int)reader["TenantId"],
                                Name = (string)reader["TenantName"]
                            };
                            property.TenantId = (int)reader["TenantId"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return property;
        }

        public RentModel GetRentById(int year, int month, int property)
        {
            RentModel rent = null;

            string sqlStatement =
                "select dbo.Rents.Year, dbo.Rents.Month, " +
                "dbo.Rents.Property as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Rents.Tenant as TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Rents.Owed, dbo.Rents.Payed, dbo.Rents.isPayed " +
                "from ((dbo.Rents " +
                "inner join dbo.Properties on dbo.Rents.Property = dbo.Properties.Id) " +
                "inner join dbo.Tenants on dbo.Rents.Tenant = dbo.Tenants.Id) " +
                "where dbo.Rents.Year = @year and dbo.Rents.Month = @month and dbo.Rents.Property = @property";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@property", property);

                try
                {
                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        rent = new RentModel
                        {
                            Year = (int)reader["Year"],
                            Month = (int)reader["Month"],
                            PropertyId = (int)reader["PropertyId"],
                            PropertyAddress = (string)reader["PropertyAddress"],
                            TenantId = (int)reader["TenantId"],
                            TenantName = (string)reader["TenantName"],
                            Owed = (int)reader["Owed"],
                            Payed = (int)reader["Payed"],
                            IsPayed = (bool)reader["isPayed"]
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return rent;
        }

        #endregion

        /*********************************************************************
         * Search For Things
         *********************************************************************/

        #region SEARCH

        public List<OwnerModel> SearchOwners(string searchTerm)
        {
            List<OwnerModel> owners = new();

            string sqlStatement = 
                "select dbo.Owners.Id as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Tenant as TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Rent " +
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

                    int curId = 0;

                    while (reader.Read())
                    {
                        if ((int)reader["OwnerId"] != curId)
                        {
                            curId = (int)reader["OwnerId"];
                            owners.Add(new OwnerModel { 
                                Id = (int)reader["OwnerId"], 
                                Name = (string)reader["OwnerName"] 
                            });
                        }
                        owners[^1].Properties.Add(new PropertyModel
                        {
                            Id = (int)reader["PropertyId"],
                            Address = (string)reader["PropertyAddress"],
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
            return owners;
        }

        public List<TenantModel> SearchTenants(string searchTerm)
        {
            List<TenantModel> tenants = new();

            string sqlStatement = 
                "select dbo.Tenants.Id as TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Rent " +
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

                    int curId = 0;

                    while (reader.Read())
                    {
                        if ((int)reader["TenantId"] != curId)
                        {
                            curId = (int)reader["TenantId"];
                            tenants.Add(new TenantModel { 
                                Id = (int)reader["TenantId"], 
                                Name = (string)reader["TenantName"] 
                            });
                        }
                        tenants[^1].Properties.Add(new PropertyModel
                        {
                            Id = (int)reader["PropertyId"],
                            Address = (string)reader["PropertyAddress"],
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
            return tenants;
        }

        public List<PropertyModel> SearchProperties(string searchTerm)
        {
            List<PropertyModel> properties = new();

            string sqlStatement = 
                "select dbo.Properties.Id as PropertyId, dbo.Properties.Address as PropertyAddress, " +
                "dbo.Properties.Owner as OwnerId, dbo.Owners.Name as OwnerName, " +
                "dbo.Properties.Tenant TenantId, dbo.Tenants.Name as TenantName, " +
                "dbo.Properties.Rent " +
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
                            Id = (int)reader["PropertyId"],
                            Address = (string)reader["PropertyAddress"],
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

                    _logger.Info("Created owner with name " + owner.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Owner creation failed. Message: " + ex.Message);
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
                    _logger.Info("Created tenant with name " + tenant.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Tenant creation failed. Message: " + ex.Message);
                }
            }
            return newId;
        }

        public int InsertProperty(PropertyModel property)
        {
            string sqlStatement = 
                "INSERT INTO dbo.Properties (Address, Owner, Tenant, Rent) " +
                "VALUES (@Address, @Owner, @Tenant, @Rent)";
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
                    _logger.Info("Created property with address " + property.Address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Property creation failed. Message: " + ex.Message);
                }
            }
            return newId;
        }

        public void CreateObligationForRent()
        {
            DateTime dateTime = DateTime.Now;
            List<PropertyModel> properties = GetProperties();

            string sqlStatement =
                "insert into dbo.Rents " +
                "(Year, Month, Property, Tenant, Owed) " +
                "values (@year, @month, @property, @tenant, @owed)";

            using (SqlConnection connection = new(connectionString))
            {
                try
                {
                    connection.Open();

                    foreach (PropertyModel property in properties)
                    {
                        if (property.TenantId != 0)
                        {
                            SqlCommand sqlCommand = new(sqlStatement, connection);
                            sqlCommand.Parameters.AddWithValue("@year", dateTime.Year.ToString());
                            sqlCommand.Parameters.AddWithValue("@month", dateTime.Month.ToString());
                            sqlCommand.Parameters.AddWithValue("@property", property.Id);
                            sqlCommand.Parameters.AddWithValue("@tenant", property.TenantId);
                            sqlCommand.Parameters.AddWithValue("@owed", property.Rent);

                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                    _logger.Info("Created monthly rents.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Monthly rent creation failed at some point. Message: " + ex.Message);
                }
            }
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
                    _logger.Info(String.Format("Owner with ID {0} was updated to {1}", owner.Id, owner.Name));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Owner update failed. Message: " + ex.Message);
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
                    _logger.Info(String.Format("Tenant with ID {0} was updated to {1}", tenant.Id, tenant.Name));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Tenant update failed. Message: " + ex.Message);
                }
            }
            return reply;
        }

        public int UpdateProperty(PropertyModel property)
        {
            int reply = -1;
            string sqlStatement = 
                "UPDATE dbo.Properties SET " +
                "Address=@Address, Owner=@Owner, Tenant=@Tenant, Rent=@Rent WHERE Id=@Id";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@Address", property.Address);
                cmd.Parameters.AddWithValue("@Owner", (property.OwnerId != 0) ? property.OwnerId : DBNull.Value);
                cmd.Parameters.AddWithValue("@Tenant", (property.TenantId != 0) ? property.TenantId : DBNull.Value);
                cmd.Parameters.AddWithValue("@Rent", property.Rent);
                cmd.Parameters.AddWithValue("@Id", property.Id);

                try
                {
                    connection.Open();
                    reply = Convert.ToInt32(cmd.ExecuteScalar());
                    _logger.Info(String.Format("Property with ID {0} was updated", property.Id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Property update failed. Message: " + ex.Message);
                }
            }
            return reply;
        }

        public int UpdateRent(int year, int month, int property, int payment)
        {
            int reply = -1;
            string sqlStatement =
                "update dbo.Rents set " +
                "Payed = Payed + @payment " +
                "where Year = @year and Month = @month and Property = @property";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@property", property);
                cmd.Parameters.AddWithValue("@payment", payment);

                try
                {
                    connection.Open();
                    reply = cmd.ExecuteNonQuery();
                    _logger.Info(String.Format("Rent with ID {0}-{1}-{2} was updated", year, month, property));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Rent update failed. Message: " + ex.Message);
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

                    reply = Convert.ToInt32(cmd.ExecuteNonQuery());
                    _logger.Info(String.Format("Owner with ID {0} was deleted", Id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Owner deletion failed. Message: " + ex.Message);
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

                    reply = Convert.ToInt32(cmd.ExecuteNonQuery());
                    _logger.Info(String.Format("Tenant with ID {0} was deleted", Id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Tenant deletion failed. Message: " + ex.Message);
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

                    reply = Convert.ToInt32(cmd.ExecuteNonQuery());
                    _logger.Info(String.Format("Property with ID {0} was deleted", Id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Property deletion failed. Message: " + ex.Message);
                }
            }
            return reply;
        }

        public int DeleteRent(int year, int month, int property)
        {
            int reply = -1;
            string sqlStatement = "DELETE FROM dbo.Rents WHERE Year = @year and Month = @month and Property = @property";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@property", property);

                try
                {
                    connection.Open();

                    reply = cmd.ExecuteNonQuery();
                    _logger.Info(String.Format("Rent with ID {0}-{1}-{2} was deleted", year, month, property));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Rent deletion failed. Message: " + ex.Message);
                }
            }
            return reply;
        }

        public void DeleteAllRent()
        {
            string sqlStatement = "delete from dbo.Rents";
            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand cmd = new(sqlStatement, connection);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    _logger.Info("Deleted all rents");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("Something went wrong during the deletion of all rents. Message: " + ex.Message);
                }
            }
        }

        #endregion


    }
}
