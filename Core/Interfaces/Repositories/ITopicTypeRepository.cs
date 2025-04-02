using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ITopicTypeRepository
    {
        Task<List<TopicType>> GetAllTopicTypesAsync();
        Task<TopicType> GetTopicByIdAsync(long topicId);
    }
}
