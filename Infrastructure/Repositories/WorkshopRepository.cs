using Core.DTOs.Workshop;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Modals;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WorkshopRepository : IWorkshopRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public WorkshopRepository(AppDbContext context, IConfiguration configuration, string connectionStringName = "ProdConnection")
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(connectionStringName);
        }

        public async Task<GenericResponse<List<WorkshopDto>>> GetAllWorkshopsAsync(string? search)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@KeyWord", search, DbType.String);
                    parameters.Add("@Operation", 0, DbType.Int64);

                    // Execute the stored procedure and return the raw results
                    var result = await connection.QueryAsync<dynamic>(
                        "sp_Workshops_Select",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var workshopDtos = result.Select(w => new WorkshopDto
                    {
                        Date = w.Date != null ? (DateTime?)w.Date : null, 
                        Hours = w.Hours != null ? (decimal?)w.Hours : null, 
                        Paid = w.Paid != null ? (bool?)w.Paid : null, 
                        PaidDate = w.PaidDate != null ? (DateTime?)w.PaidDate : null, 
                        Sponsor = w.Sponsor, 
                        TypeID = w.TypeID != null ? (long?)w.TypeID : null, 
                        WorkshopID = Convert.ToInt64(w.WorkshopID), 
                        WorkshopName = w.WorkshopName, 
                        TopicNames = w.TopicNames, 
                        MemberCount = Convert.ToInt32(w.Members), 
                        TopicCount = Convert.ToInt32(w.Topics) 
                    }).ToList();



                    return new GenericResponse<List<WorkshopDto>>(true, "All workshops retrieved successfully.", workshopDtos);
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<WorkshopDto>>(false, ex.Message, null);
            }
        }




        public IQueryable<Workshop> GetAllWorkshops()
        {
            return _context.Workshops
                  .Include(w => w.WorkshopMembers)
                  .Include(w => w.WorkshopTopics)
                  .AsQueryable();
        }

        public async Task<Workshop> GetWorkshopByIdAsync(long id)
        {
            return await _context.Workshops
                .Include(w => w.WorkshopMembers)
                .Include(w => w.WorkshopTopics)
                .FirstOrDefaultAsync(w => w.WorkshopID == id);
        }


        public async Task<Workshop> AddWorkshopAsync(Workshop workshop)
        {
            _context.Workshops.Add(workshop);
            await _context.SaveChangesAsync();
            return workshop;
        }

        public async Task<Workshop> UpdateWorkshopAsync(Workshop workshop)
        {
            var existingWorkshop = await _context.Workshops.FindAsync(workshop.WorkshopID);
            if (existingWorkshop == null)
            {
                return null;
            }

            _context.Entry(existingWorkshop).CurrentValues.SetValues(workshop);
            await _context.SaveChangesAsync();
            return existingWorkshop;
        }

        public async Task<bool> DeleteWorkshopAsync(long id)
        {
            var workshop = await _context.Workshops.FindAsync(id);
            if (workshop == null)
            {
                return false;
            }

            _context.Workshops.Remove(workshop);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task BulkInsertWorkshopsAsync(IEnumerable<Workshop> workshops)
        {
            await _context.Workshops.AddRangeAsync(workshops);
            await _context.SaveChangesAsync();
        }

        public async Task BulkInsertWorkshopTopicsAsync(IEnumerable<WorkshopTopic> workshopTopics)
        {
            await _context.WorkshopTopics.AddRangeAsync(workshopTopics);
            await _context.SaveChangesAsync();
        }

        public async Task BulkInsertWorkshopMembersAsync(IEnumerable<WorkshopMember> workshopMembers)
        {
            await _context.WorkshopMembers.AddRangeAsync(workshopMembers);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveWorkshopTopicsAsync(IEnumerable<long> topicIds)
        {
            var topicsToRemove = _context.WorkshopTopics.Where(wt => topicIds.Contains(wt.WorkshopTopicID));
            _context.WorkshopTopics.RemoveRange(topicsToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveWorkshopMembersAsync(IEnumerable<long> memberIds)
        {
            var membersToRemove = _context.WorkshopMembers.Where(wm => memberIds.Contains(wm.WorkshopMemberID));
            _context.WorkshopMembers.RemoveRange(membersToRemove);
            await _context.SaveChangesAsync();
        }
    }

}
