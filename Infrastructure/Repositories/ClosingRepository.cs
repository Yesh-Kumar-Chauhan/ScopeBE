using AutoMapper;
using Core.DTOs;
using Core.DTOs.Core.DTOs;
using Core.DTOs.Sites;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ClosingRepository : IClosingRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClosingRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<Closing> GetAllClosings()
        {
            return _context.Closings.AsQueryable();
        }

        public async Task<List<Closing>> GetAllClosingsAsync()
        {
            return await _context.Closings.ToListAsync();
        }

        public async Task<Closing?> GetClosingByIdAsync(long id)
        {
            return await _context.Closings.FindAsync(id);
        }

        public async Task<Closing> AddClosingAsync(Closing closing)
        {
            _context.Closings.Add(closing);
            await _context.SaveChangesAsync();
            return closing;
        }

        public async Task<Closing?> UpdateClosingAsync(Closing closing)
        {
            var existingClosing = await _context.Closings.FindAsync(closing.ClosingID);
            if (existingClosing == null)
                return null;

            _context.Entry(existingClosing).CurrentValues.SetValues(closing);
            await _context.SaveChangesAsync();
            return existingClosing;
        }

        public async Task<bool> DeleteClosingAsync(long id)
        {
            var closing = await _context.Closings.FindAsync(id);
            if (closing == null)
                return false;

            _context.Closings.Remove(closing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ClosingDto>> GetClosingsByDistrictIdAsync(long? districtId)
        {
            var closingsDto = new List<ClosingDto>();

            using (var connection = _context.Database.GetDbConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_Closings_Select";
                    command.Parameters.Add(new SqlParameter("@DistrictID", districtId.HasValue ? (object)districtId.Value : DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var closing = new ClosingDto
                            {
                                ClosingID = reader.GetInt64(reader.GetOrdinal("ClosingID")),
                                DistrictID = reader.IsDBNull(reader.GetOrdinal("DistrictID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("DistrictID")),
                                DATE = reader.IsDBNull(reader.GetOrdinal("DATE")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DATE")),
                                STATUS = reader.IsDBNull(reader.GetOrdinal("STATUS")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("STATUS")),
                                ParentCredit = reader.GetBoolean(reader.GetOrdinal("PARENT_CR")),
                                STAFF_PH = reader.GetBoolean(reader.GetOrdinal("STAFF_PH")),
                                STAFF_DT = reader.GetBoolean(reader.GetOrdinal("STAFF_DT")),
                                StaffPaid = reader.GetBoolean(reader.GetOrdinal("STAFF_ALL")),
                                NOTES = reader.IsDBNull(reader.GetOrdinal("NOTES")) ? null : reader.GetString(reader.GetOrdinal("NOTES")),
                                MakeUpDay = reader.GetBoolean(reader.GetOrdinal("MakeUpDay")),
                                // Assuming StatusName is a property in ClosingDto for the name from Status table
                                StatusName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? null : reader.GetString(reader.GetOrdinal("StatusName"))
                            };

                            closingsDto.Add(closing);
                        }
                    }
                }
            }

            return closingsDto;
        }

    }
}
