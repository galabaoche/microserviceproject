using System;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Dtos;
using Contact.API.IntegrationEvents.Events;
using DotNetCore.CAP;

namespace Contact.API.IntegrationEvents.EventHandling
{

    public class UserProfileChangedEventHandler //: ICapSubscribe
    {
        private readonly IContactRepository _contactRepository;

        public UserProfileChangedEventHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [CapSubscribe("userapi.user.profile.changed")]
        public async Task UpdateContactInfo(UserInfoChangedEvent @event)
        {
            var token = new CancellationToken();
            await _contactRepository.UpdateContactInfoAsync(new UserIdentity
            {
                UserId = @event.UserId,
                Avatar = @event.Avatar,
                Company = @event.Company,
                Name = @event.Name,
                Phone = @event.Phone,
                Title = @event.Title
            }, token);
        }
    }
}
