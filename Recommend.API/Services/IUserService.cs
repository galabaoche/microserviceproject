using System;
using System.Threading.Tasks;
using Recommend.API.Dtos;

namespace Recommend.API.Services
{
    public interface IUserService
    {
        Task<UserIdentity> GetUserIdentityAsync(int userId);
    }
}
