using System;
using System.Threading.Tasks;

namespace Project.API.Application.Service
{
   public interface IRecommendService
    {
        Task<bool> IsProjectInRecommend(int projectId,int userId);
    }
}
