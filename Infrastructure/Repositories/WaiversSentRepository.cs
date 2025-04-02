using Core.DTOs.Personel;
using Core.Entities;
using Core.Interfaces.Repositories;
using EFCore.BulkExtensions;
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
    public class WaiversSentRepository : IWaiversSentRepository
    {
        private readonly AppDbContext _context;

        public WaiversSentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WaiversSent>> GetWaiversByStaffIdAsync(long staffId)
        {
            return await _context.WaiversSent
                .Where(w => w.STAFF_ID == staffId || staffId == 0)
                .OrderBy(w => w.SENT)
                .ToListAsync();
        }

        public async Task<WaiversSent> GetByIdAsync(long waiverId)
        {
            return await _context.WaiversSent.FindAsync(waiverId);
        }

        public async Task<WaiversSent> AddAsync(WaiversSent waiver)
        {
            _context.WaiversSent.Add(waiver);
            await _context.SaveChangesAsync();
            return waiver;
        }

        public async Task<WaiversSent> UpdateAsync(WaiversSent waiver)
        {
            _context.WaiversSent.Update(waiver);
            await _context.SaveChangesAsync();
            return waiver;
        }

        public async Task<bool> DeleteAsync(long waiverId)
        {
            var waiver = await _context.WaiversSent.FindAsync(waiverId);
            if (waiver == null)
            {
                return false;
            }

            _context.WaiversSent.Remove(waiver);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<WaiversSent> GetAllWaivers()
        {
            return _context.WaiversSent.AsQueryable();
        }

        public async Task<List<WaiversSent>> GetAllWaiversAsync()
        {
            return await _context.WaiversSent.ToListAsync();
        }

        public async Task<List<WaiversReceivedDto>> GetWaiversReceivedByStaffId(long personId)
        {
            var certificateDtos = new List<WaiversReceivedDto>();

            using (var connection = _context.Database.GetDbConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[sp_WaiversReceiveds_Select]";
                    command.Parameters.Add(new SqlParameter("@PersonID", personId));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var certificateDto = new WaiversReceivedDto
                            {
                                DistName = reader.IsDBNull(reader.GetOrdinal("DIST_NAM")) ? null : reader.GetString(reader.GetOrdinal("DIST_NAM")),
                                SiteName = reader.IsDBNull(reader.GetOrdinal("SITE_NAM")) ? null : reader.GetString(reader.GetOrdinal("SITE_NAM")),
                                SiteID = reader.IsDBNull(reader.GetOrdinal("SiteID")) ? null : reader.GetInt64(reader.GetOrdinal("SiteID")),
                                Country = reader.IsDBNull(reader.GetOrdinal("County")) ? null : reader.GetString(reader.GetOrdinal("County")),
                                WaiversReceivedID = reader.IsDBNull(reader.GetOrdinal("WaiversReceivedID")) ? null : reader.GetInt64(reader.GetOrdinal("WaiversReceivedID")),
                                StaffID = reader.IsDBNull(reader.GetOrdinal("STAFF_ID")) ? null : reader.GetInt64(reader.GetOrdinal("STAFF_ID")),
                                Received = reader.IsDBNull(reader.GetOrdinal("RECEIVED")) ? null : reader.GetDateTime(reader.GetOrdinal("RECEIVED"))
                            };

                            certificateDtos.Add(certificateDto);
                        }
                    }


                }
            }

            return certificateDtos;
        }


        public async Task<bool> AddOrUpdateWaiversReceivedAsync(List<WaiversReceived> waivers)
        {
            try
            {
                foreach (var waiver in waivers)
                {
                    // Determine if the record is new or existing based on WaiversReceivedID
                    if (waiver.WaiversReceivedID != 0 && waiver.WaiversReceivedID != null)
                    {
                        // Existing record: decide to update or delete
                        if (waiver.RECEIVED == DateTime.MinValue)
                        {
                            // Mark for deletion if RECEIVED is DateTime.MinValue
                            _context.Entry(waiver).State = EntityState.Deleted;
                        }
                        else
                        {
                            // Mark for update
                            _context.Entry(waiver).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        // New record: only add if RECEIVED is not DateTime.MinValue
                        if (waiver.RECEIVED != DateTime.MinValue)
                        {
                            await _context.WaiversReceived.AddAsync(waiver);
                        }
                    }
                }

                // Save all changes in a single transaction
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Handle exceptions as needed, such as logging
                throw;
            }
        }



    }
}
