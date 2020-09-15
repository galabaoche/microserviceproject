using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dtos;

namespace Contact.API.Data
{
    public interface IContactRepository
    {
        /// <summary>
        /// 更新联系人信息
        /// </summary>
        /// <param name="userIdentity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UpdateContactInfoAsync(UserIdentity userIdentity, CancellationToken cancellationToken);
        /// <summary>
        /// 增加联系人信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userIdentity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddContactAsync(int userId, UserIdentity userIdentity, CancellationToken cancellationToken);
        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Models.Contact>> GetContactAsync(int userId, CancellationToken cancellationToken);
        Task<bool> TagContactAsync(int userId, int contactId, List<string> tags, CancellationToken cancellationToken);
    }
}
