using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Recommend.API.Data;
using Recommend.API.IntegrationEvents;
using Recommend.API.Models;
using Recommend.API.Services;

namespace Recommend.API.InetgrationEventsHandler
{
    public class ProjectCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly RecommendDbContext _recommendDbContext;
        private readonly IContactService _contactService;
        private readonly IUserService _userService;

        public ProjectCreatedIntegrationEventHandler(RecommendDbContext recommendDbContext, IUserService userService, IContactService contactService)
        {
            _recommendDbContext = recommendDbContext;
            _userService = userService;
            _contactService = contactService;
        }

        [CapSubscribe("projectapi.project.created")]
        public async Task CreateRecommendFromProject(ProjectCreatedIntegrationEvent @event)
        {
            var fromUser = await _userService.GetUserIdentityAsync(@event.UserId);
            var contacts = await _contactService.GetContactsByUserIdAsync(@event.UserId);
            foreach (var contact in contacts)
            {
                var recommend = new ProjectRecommend
                {
                    FromUserId = @event.UserId,
                    Company = @event.Company,
                    Tags = @event.Tags,
                    ProjectId = @event.ProjectId,
                    ProjectAvatar = @event.ProjectAvatar,
                    FinStage = @event.FinStage,
                    ReCommendTime = DateTime.Now,
                    CreatedTime = @event.CreatedTime,
                    Introduction = @event.Introduction,
                    ReCommendType = EnumReCommendType.Friend,
                    FromUserAvatar = fromUser.Avatar,
                    FromUserName = fromUser.Name,
                    UserId = contact.UserId
                };
                _recommendDbContext.Add(recommend);
            }
            await _recommendDbContext.SaveChangesAsync();
        }
    }
}
