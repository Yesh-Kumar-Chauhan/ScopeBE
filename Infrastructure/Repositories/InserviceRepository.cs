using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.Inservice;
using Core.Entities;
using Core.Interfaces.Repositories;
using EFCore.BulkExtensions;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class InserviceRepository : IInserviceRepository
{
    private readonly AppDbContext _context;
    private readonly string _connectionString;

    public InserviceRepository(AppDbContext context, IConfiguration configuration, string connectionStringName = "ProdConnection")
    {
        _context = context;
        _connectionString = configuration.GetConnectionString(connectionStringName);
    }
    public IQueryable<Inservice> GetAllInservices()
    {
        return _context.Inservices.AsQueryable();
    }

    public async Task<List<Inservice>> GetAllInservicesAsync()
    {
        return await _context.Inservices.ToListAsync();
    }

    public async Task<Inservice> GetInserviceByIdAsync(long id)
    {
        return await _context.Inservices.FindAsync(id);
    }

    public async Task<Inservice> AddInserviceAsync(Inservice inservice)
    {
        _context.Inservices.Add(inservice);
        await _context.SaveChangesAsync();
        return inservice;
    }

    public async Task<Inservice> UpdateInserviceAsync(Inservice inservice)
    {
        var existingInservice = await _context.Inservices.FindAsync(inservice.InserviceID);
        if (existingInservice == null)
        {
            return null;
        }

        _context.Entry(existingInservice).CurrentValues.SetValues(inservice);
        await _context.SaveChangesAsync();
        return existingInservice;
    }

    public async Task<bool> DeleteInserviceAsync(long id)
    {
        var inservice = await _context.Inservices.FindAsync(id);
        if (inservice == null)
        {
            return false;
        }

        _context.Inservices.Remove(inservice);
        await _context.SaveChangesAsync();
        return true;
    }

    // Method for bulk inserting inservices
    public async Task BulkInsertInservicesAsync(List<Inservice> inservices)
    {
        await _context.BulkInsertAsync(inservices);
    }

    public async Task<List<InserviceDto>> GetInserviceSelectAsync(long? personId, int operation)
    {
        var inservices = new List<InserviceDto>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand("sp_Inservices_Select", connection))
            {
                // Log parameter values for debugging
                Console.WriteLine($"PersonID: {personId}, Operation: {operation}");
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.BigInt)
                {
                    Value = personId == 0 ? DBNull.Value : (object)personId.ToString()
                });

                command.Parameters.Add(new SqlParameter("@Operation", SqlDbType.BigInt)
                {
                    Value = operation
                });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    //var newReader = await reader.ReadAsync();
                    while (await reader.ReadAsync())
                    {
                        var inservice = new InserviceDto
                        {
                            InserviceID = reader.GetInt64(reader.GetOrdinal("InserviceID")),
                            StaffId = reader.IsDBNull(reader.GetOrdinal("STAFF_ID")) ? null : reader.GetInt64(reader.GetOrdinal("STAFF_ID")),
                            Training = reader.IsDBNull(reader.GetOrdinal("TRAINING")) ? null : reader.GetString(reader.GetOrdinal("TRAINING")),
                            Date = reader.IsDBNull(reader.GetOrdinal("DATE")) ? null : reader.GetDateTime(reader.GetOrdinal("DATE")),
                            Hours = reader.IsDBNull(reader.GetOrdinal("HOURS")) ? null : reader.GetDecimal(reader.GetOrdinal("HOURS")),
                            TopicId = reader.IsDBNull(reader.GetOrdinal("TopicID")) ? null : reader.GetInt64(reader.GetOrdinal("TopicID")),
                            Sponsor = reader.IsDBNull(reader.GetOrdinal("SPONSOR")) ? null : reader.GetString(reader.GetOrdinal("SPONSOR")),
                            WorkshopTypeId = reader.IsDBNull(reader.GetOrdinal("WorkShopTypeID")) ? null : reader.GetInt64(reader.GetOrdinal("WorkShopTypeID")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("NOTES")) ? null : reader.GetString(reader.GetOrdinal("NOTES")),
                            Flag = reader.IsDBNull(reader.GetOrdinal("Flag")) ? null : reader.GetString(reader.GetOrdinal("Flag")),
                            Paid = reader.GetBoolean(reader.GetOrdinal("Paid")),
                            PaidDate = reader.IsDBNull(reader.GetOrdinal("PaidDate")) ? null : reader.GetDateTime(reader.GetOrdinal("PaidDate")),
                        };
                        inservices.Add(inservice);
                    }
                }
            }
        }

        return inservices;
    }
}
