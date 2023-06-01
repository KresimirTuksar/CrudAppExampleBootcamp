using ExampleApp.Model;
using ExampleApp.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApp.Repository
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=FapMash1na;Database=Advertisement;";

        public List<AdModel> GetAllAds()
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
                    NpgsqlDataReader reader = cmd.ExecuteReader();

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

        public void GetAllAdsCategories() { }

        public AdModel GetAdById(Guid id)
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

                    NpgsqlDataReader reader = cmd.ExecuteReader();

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
        public bool CreateAd(AdModel request)
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

                        int adsRowsAffected = createAdCmd.ExecuteNonQuery();

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


            return true;
        }
        public void EditAd() { }
        public void DeleteAd() { }

    }
}
