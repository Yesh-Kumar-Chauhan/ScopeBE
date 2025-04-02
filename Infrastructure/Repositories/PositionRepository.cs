using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly IConfiguration _configuration;

        public PositionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Positions>> GetPositionsByTypeAsync(string? type)
        {
            var positions = new List<Positions>();
            string connectionString = _configuration.GetConnectionString("ProdConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_Positions_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Type", SqlDbType.NVarChar) { Value = string.IsNullOrEmpty(type) ? "" : type });

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            positions.Add(new Positions
                            {
                                PositionId = reader["PositionID"] != DBNull.Value ? (int?)reader["PositionID"] : null,
                                Position = reader["Position"] != DBNull.Value ? reader["Position"].ToString() : "",
                                Type = reader["Type"] != DBNull.Value ? reader["Type"].ToString() : ""
                            });
                        }
                    }
                }
            }

            return positions;
        }
    }
}
