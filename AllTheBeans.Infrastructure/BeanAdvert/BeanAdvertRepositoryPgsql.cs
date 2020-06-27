using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Helpers;
using AllTheBeans.Core.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Infrastructure.BeanAdvert
{
    public class BeanAdvertRepositoryPgsql : IBeanAdvertRepository
    {
        static NpgsqlConnection _sqlConnection;
        string connString = null;

        public BeanAdvertRepositoryPgsql()
        {
            // Read connstring from environment variable
            connString = EnvironmentConfigHelper.GetDbConnectionString();

            if (connString == null)
                throw new NullReferenceException("Connection String is null");

            _sqlConnection = new NpgsqlConnection(connString);

            // We'll block the thread that has been opened for this async func. 
            // The connection needs to be opened. So we'll wait until timeout
            // TODO: Handle timeout
            _sqlConnection.OpenAsync().Wait();
        }



        public async Task AddAdvert(BeanCreateAdvertDTO advert)
        {
            var formattedDate = advert.Date.ToString("yyyy MM dd");
            var formattedString = formattedDate.Replace(" ", "-");

            // SQL
            try
            {
                using (var cmd = new NpgsqlCommand($"INSERT INTO beanadverts(bean_id,advert_date) VALUES({advert.BeanId},'{formattedString}')", _sqlConnection))
                {
                    await cmd.ExecuteNonQueryAsync();
                    cmd.Dispose();
                    _sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Post errors to a logging service
                Console.WriteLine(ex.InnerException);
            }
        }

        public async Task ClearAdverts()
        {
            try
            {
                using (var cmd = new NpgsqlCommand($"DELETE from beanadverts", _sqlConnection))
                {
                    await cmd.ExecuteNonQueryAsync();
                    cmd.Dispose();
                    _sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Post errors to a logging service
                Console.WriteLine(ex.InnerException);
            }
        }

        public async Task<BeanDTO> GetAdvert(DateTime date)
        {
            var formattedDate = date.ToString("yyyy MM dd");
            var formattedString = formattedDate.Replace(" ", "-");

            var bean = new BeanDTO();
            // SQL
            try
            {
                using (var cmd = new NpgsqlCommand(
                    $"SELECT bean_name, aroma, colour, imageurl, price " +
                    $"FROM beans " +
                    $"INNER JOIN beanadverts ON beans.bean_id = beanadverts.bean_id " +
                    $"WHERE beanadverts.advert_date = '{formattedString}'",
                    _sqlConnection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            bean.Name = reader["bean_name"].ToString();
                            bean.Aroma = reader["aroma"].ToString();
                            bean.Colour = reader["colour"].ToString();
                            bean.ImageUrl = reader["imageurl"].ToString();
                            bean.CostPer100g = (decimal)reader["price"];
                        }
                    }

                    _sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Post errors to a logging service
                Console.WriteLine(ex.InnerException);
            }

            return bean.IsValid() ? bean : null;
        }
    }
}
