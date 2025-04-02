using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TopicTypeRepository : ITopicTypeRepository
    {
        private readonly AppDbContext _context;

        public TopicTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TopicType>> GetAllTopicTypesAsync()
        {
            return await _context.TopicType.ToListAsync();
        }

        public async Task<TopicType> GetTopicByIdAsync(long topicId)
        {
            return await _context.TopicType.FindAsync(topicId);
        }
    }
}
