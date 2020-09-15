using System;
using System.Threading.Tasks;
using User.Identity.Dtos;

namespace User.Identity.Services
{
    public interface IUserService
    {
        Task<UserIdentity> CheckOrCreate(string phone);
    }
}

