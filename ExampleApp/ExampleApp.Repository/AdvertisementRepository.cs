using ExampleApp.Model;
using ExampleApp.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExampleApp.Repository
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=FapMash1na;Database=Advertisement;";

        public async Task<List<AdModel>> GetAllAdsAsync()
        {
            List<AdModel> result = new List<AdModel>();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = $"SELECT * FROM \"Ads\"";
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        DateTime? updatedAt;

                        if (!reader.IsDBNull(reader.GetOrdinal("UpdatedAt")))
                        {
                            updatedAt = reader.GetFieldValue<DateTime>(reader.GetOrdinal("UpdatedAt"));
                        }
                        else
                        {
                            updatedAt = null;
                        }

                        reader.GetOrdinal("UpdatedAt");
                        result.Add(new AdModel()
                        {
                            Id = Guid.Parse(reader["Id"].ToString()),
                            UserId = Guid.Parse(reader["UserId"].ToString()),
                            Content = reader["Content"].ToString(),
                            CreatedAt = reader.GetFieldValue<DateTime>(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = updatedAt
                        }); ;
                    }
                    if (result.Count() == 0)
                    {
                        return new List<AdModel>();

                    }
                    return result;
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<List<AdCategoryModel>> GetAllAdsCategoriesAsync() {

            List<AdCategoryModel> result = new List<AdCategoryModel>();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    //cmd.CommandText = $"SELECT * FROM \"Ads\"";
                    //cmd.CommandText = "SELECT \"Ads.Id\", \"Ads.Content\", \"Category.Name\" " +
                    //                "FROM \"Ads\" " +
                    //                "INNER JOIN \"AdCategory\" ON \"AdCategory.AdId\" = \"Ads.Id\" " +
                    //                "INNER JOIN \"Category\" ON \"AdCategory.CategoryId\" = \"Category.Id\" ";

                    cmd.CommandText = "select \"Ads\".\"Id\", \"Ads\".\"Content\",\"Users\".\"FirstName\" , \"Users\".\"LastName\", \"Category\".\"Name\" from \"Ads\" inner join \"Users\" on \"Users\".\"Id\" = \"Ads\".\"UserId\" inner join \"AdCategory\" on \"AdCategory\".\"AdId\" = \"Ads\".\"Id\" inner join \"Category\" on \"Category\".\"Id\" = \"AdCategory\".\"CategoryId\" ";
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(new AdCategoryModel()
                        {
                            Id = Guid.Parse(reader["Id"].ToString()),
                            UserFirstName = reader["FirstName"].ToString(),
                            UserLastName = reader["LastName"].ToString(),
                            Content = reader["Content"].ToString(),
                            Category = reader["Name"].ToString(),
                        }); ;
                    }
                    if (result.Count() == 0)
                    {
                        return new List<AdCategoryModel>();

                    }
                    return result;
                }
            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public async Task<AdModel> GetAdByIdAsync(Guid id)
        {

            AdModel result = new AdModel();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM \"Ads\" WHERE \"Id\" = @adGuid";

                    cmd.Parameters.AddWithValue("adGuid", id);

                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        DateTime? updatedAt;

                        if (!reader.IsDBNull(reader.GetOrdinal("UpdatedAt")))
                        {
                            updatedAt = reader.GetFieldValue<DateTime>(reader.GetOrdinal("UpdatedAt"));
                        }
                        else
                        {
                            updatedAt = null;
                        }

                        result = new AdModel()
                        {
                            Id = Guid.Parse(reader["Id"].ToString()),
                            UserId = Guid.Parse(reader["UserId"].ToString()),
                            Content = reader["Content"].ToString(),
                            CreatedAt = reader.GetFieldValue<DateTime>(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = updatedAt
                        };
                    }

                    return result;
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<bool> CreateAdAsync(AdModel request)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand createAdCmd = new NpgsqlCommand())
                    {
                        createAdCmd.Connection = conn;
                        createAdCmd.CommandText = "INSERT INTO \"Ads\" ( \"Id\", \"UserId\", \"Content\")" +
                                                        "VALUES( @adGuid, @UserId, @Content);";

                        createAdCmd.Parameters.AddWithValue("adGuid", request.Id);
                        createAdCmd.Parameters.AddWithValue("userId", request.UserId);
                        createAdCmd.Parameters.AddWithValue("content", request.Content);

                        int adsRowsAffected = await createAdCmd.ExecuteNonQueryAsync();

                        if (adsRowsAffected > 0)
                        {
                            return true;
                        }
                        return false;
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> EditAdAsync(AdModel request)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (NpgsqlCommand selectCmd = new NpgsqlCommand())
                    {
                        selectCmd.Connection = conn;
                        selectCmd.CommandText = "SELECT * FROM \"Ads\" WHERE \"Id\" = @adGuid";

                        selectCmd.Parameters.AddWithValue("adGuid", request.Id);


                        using (NpgsqlDataReader reader = await selectCmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                AdModel ad = new AdModel()
                                {
                                    Id = Guid.Parse(reader["Id"].ToString()),
                                    Content = reader["Content"].ToString(),
                                };

                                reader.Close();

                                if (ad.Content != request.Content)
                                {

                                    using (NpgsqlCommand updateCmd = new NpgsqlCommand())
                                    {
                                        updateCmd.Connection = conn;

                                        updateCmd.CommandText = "UPDATE \"Ads\" SET \"Content\" = @content, \"UpdatedAt\" = @updatedAt WHERE \"Id\" = @adGuid";

                                        updateCmd.Parameters.AddWithValue("adGuid", request.Id);
                                        updateCmd.Parameters.AddWithValue("content", request.Content);
                                        updateCmd.Parameters.AddWithValue("updatedAt", request.UpdatedAt);


                                        int rowsAffected = await updateCmd.ExecuteNonQueryAsync();

                                        if (rowsAffected > 0)
                                        {
                                            return true;
                                        }
                                        return false;
                                    }
                                }
                                else
                                {
                                    //return Request.CreateResponse(HttpStatusCode.NoContent, "Nothing to do"); //needs to be handled in another way
                                    return true;
                                }
                            }
                            //return Request.CreateResponse(HttpStatusCode.NotFound, "Ad Not Found"); // needs to be handled in another way
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<bool> DeleteAdAsync(Guid id)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand();

                    command.Connection = conn;

                    command.CommandText = "DELETE FROM \"Ads\" WHERE \"Id\" = @adId";

                    command.Parameters.AddWithValue("adId", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

    }
}
