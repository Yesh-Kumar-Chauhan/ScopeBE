using Core.DTOs.Personel;
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
    public class DirectorRepository : IDirectorRepository
    {
        private readonly AppDbContext _context;

        public DirectorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DirectorDto>> GetDirectorsByPersonIdAsync(long personId)
        {
            var directorDtos = new List<DirectorDto>();

            using (var connection = _context.Database.GetDbConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[sp_Directors_Select]";
                    command.Parameters.Add(new SqlParameter("@PersonID", personId));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var directorDto = new DirectorDto
                            {
                                DirectorID = reader.GetInt64(reader.GetOrdinal("DirectorID")),
                                PersonID = reader.GetInt64(reader.GetOrdinal("PersonID")),
                                SiteID = reader.GetInt64(reader.GetOrdinal("SiteID")),
                                MonAMFrom = reader.IsDBNull(reader.GetOrdinal("MonAMFrom")) ? null : reader.GetString(reader.GetOrdinal("MonAMFrom")),
                                MonAMTo = reader.IsDBNull(reader.GetOrdinal("MonAMTo")) ? null : reader.GetString(reader.GetOrdinal("MonAMTo")),
                                TueAMFrom = reader.IsDBNull(reader.GetOrdinal("TueAMFrom")) ? null : reader.GetString(reader.GetOrdinal("TueAMFrom")),
                                TueAMTo = reader.IsDBNull(reader.GetOrdinal("TueAMTo")) ? null : reader.GetString(reader.GetOrdinal("TueAMTo")),
                                WedAMFrom = reader.IsDBNull(reader.GetOrdinal("WedAMFrom")) ? null : reader.GetString(reader.GetOrdinal("WedAMFrom")),
                                WedAMTo = reader.IsDBNull(reader.GetOrdinal("WedAMTo")) ? null : reader.GetString(reader.GetOrdinal("WedAMTo")),
                                ThuAMFrom = reader.IsDBNull(reader.GetOrdinal("ThuAMFrom")) ? null : reader.GetString(reader.GetOrdinal("ThuAMFrom")),
                                ThuAMTo = reader.IsDBNull(reader.GetOrdinal("ThuAMTo")) ? null : reader.GetString(reader.GetOrdinal("ThuAMTo")),
                                FriAMFrom = reader.IsDBNull(reader.GetOrdinal("FriAMFrom")) ? null : reader.GetString(reader.GetOrdinal("FriAMFrom")),
                                FriAMTo = reader.IsDBNull(reader.GetOrdinal("FriAMTo")) ? null : reader.GetString(reader.GetOrdinal("FriAMTo")),
                                MonPMFrom = reader.IsDBNull(reader.GetOrdinal("MonPMFrom")) ? null : reader.GetString(reader.GetOrdinal("MonPMFrom")),
                                MonPMTo = reader.IsDBNull(reader.GetOrdinal("MonPMTo")) ? null : reader.GetString(reader.GetOrdinal("MonPMTo")),
                                TuePMFrom = reader.IsDBNull(reader.GetOrdinal("TuePMFrom")) ? null : reader.GetString(reader.GetOrdinal("TuePMFrom")),
                                TuePMTo = reader.IsDBNull(reader.GetOrdinal("TuePMTo")) ? null : reader.GetString(reader.GetOrdinal("TuePMTo")),
                                WedPMFrom = reader.IsDBNull(reader.GetOrdinal("WedPMFrom")) ? null : reader.GetString(reader.GetOrdinal("WedPMFrom")),
                                WedPMTo = reader.IsDBNull(reader.GetOrdinal("WedPMTo")) ? null : reader.GetString(reader.GetOrdinal("WedPMTo")),
                                ThuPMFrom = reader.IsDBNull(reader.GetOrdinal("ThuPMFrom")) ? null : reader.GetString(reader.GetOrdinal("ThuPMFrom")),
                                ThuPMTo = reader.IsDBNull(reader.GetOrdinal("ThuPMTo")) ? null : reader.GetString(reader.GetOrdinal("ThuPMTo")),
                                FriPMFrom = reader.IsDBNull(reader.GetOrdinal("FriPMFrom")) ? null : reader.GetString(reader.GetOrdinal("FriPMFrom")),
                                FriPMTo = reader.IsDBNull(reader.GetOrdinal("FriPMTo")) ? null : reader.GetString(reader.GetOrdinal("FriPMTo")),
                                DistName = reader.IsDBNull(reader.GetOrdinal("DIST_NAM")) ? null : reader.GetString(reader.GetOrdinal("DIST_NAM")),
                                SiteName = reader.IsDBNull(reader.GetOrdinal("SITE_NAM")) ? null : reader.GetString(reader.GetOrdinal("SITE_NAM")),
                            };

                            directorDtos.Add(directorDto);
                        }
                    }

                    await connection.CloseAsync();
                }
            }

            return directorDtos;
        }


        public async Task<Director> GetByIdAsync(long directorId)
        {
            return await _context.Directors.FindAsync(directorId);
        }

        public async Task<Director> AddAsync(Director director)
        {
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
            return director;
        }

        public async Task<Director> UpdateAsync(Director director)
        {
            _context.Directors.Update(director);
            await _context.SaveChangesAsync();
            return director;
        }

        public async Task<bool> DeleteAsync(long directorId)
        {
            var director = await _context.Directors.FindAsync(directorId);
            if (director == null)
            {
                return false;
            }

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
