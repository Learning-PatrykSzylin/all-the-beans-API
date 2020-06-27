using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Helpers;
using AllTheBeans.Core.Interfaces;
using AllTheBeans.Core.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Infrastructure.Beans
{
    public class BeanRepositryPgsql : IBeanRepository
    {
        static NpgsqlConnection _sqlConnection;
        string connString = null;

        public BeanRepositryPgsql()
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

        public async void AddBean(BeanCreateDTO bean)
        {
            // SQL
            try
            {
                using (var cmd = new NpgsqlCommand(
                    $"INSERT INTO beans(bean_name,price,aroma,colour,imageUrl) " +
                    $"VALUES (" +
                        $"'{bean.Name}', " +
                        $"{bean.CostPer100g}, " +
                        $"'{bean.Aroma}', " +
                        $"'{bean.Colour}', " +
                        $"'{bean.ImageUrl}'" +
                    $")", _sqlConnection))
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

        public async Task<BeanDTO> GetBeanById(int id)
        {
            var bean = new BeanDTO();

            using (var cmd = new NpgsqlCommand(
                $"SELECT bean_name, colour, price, imageurl, aroma " +
                $"FROM beans " +
                $"WHERE bean_id = {id}", _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        bean = new BeanDTO
                        {
                            Aroma = reader["aroma"].ToString(),
                            Colour = reader["colour"].ToString(),
                            CostPer100g = (decimal)reader["price"],
                            ImageUrl = reader["imageurl"].ToString(),
                            Name = reader["bean_name"].ToString()
                        };
                    }

                    cmd.Dispose();
                    _sqlConnection.Close();
                }

            }

            return bean;
        }

        public async Task<List<Bean>> GetAllBeans()
        {
            List<Bean> beans = new List<Bean>();
            using (var cmd = new NpgsqlCommand($"SELECT * from Beans", _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        beans.Add(new Bean
                        {       
                            BeanId = (int)reader["bean_id"],
                            Aroma = reader["aroma"].ToString(),
                            Colour = reader["colour"].ToString(),
                            CostPer100g = (decimal)reader["price"],
                            ImageUrl = reader["imageurl"].ToString(),
                            Name = reader["bean_name"].ToString()
                        });
                    }
                    cmd.Dispose();
                }

                _sqlConnection.Close();
            }

            return beans;
        }

        public async Task DeleteAllBeans()
        {
            try
            {
                using (var cmd = new NpgsqlCommand($"DELETE from beans", _sqlConnection))
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
    }
}
