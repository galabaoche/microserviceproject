using System;
using System.Threading.Tasks;
using Contact.API.Dtos;

namespace Contact.API.Services
{
    public interface IUserService
    {
        Task<UserIdentity> GetUserIdentityAsync(int userId);
    }
}
