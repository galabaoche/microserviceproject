using System;
using System.Threading.Tasks;

namespace Project.API.Application.Queries
{
    public interface IProjectQueries
    {
        Task<dynamic> GetProjectByUserIdAsync(int userId);
        Task<dynamic> GetProjectDetailAsync(int projectId);
    }
}
