using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.API.Data;
using User.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using User.API.Utilities;
using System.Collections;
using AutoMapper;
using User.API.Dtos;
using DotNetCore.CAP;

namespace User.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserContext _userContext;
        public readonly ILogger<UserController> _logger;
        public readonly IMapper _mapper;
        private readonly ICapPublisher _capPublisher;


        public UserController(UserContext userContext, ILogger<UserController> logger, IMapper mapper, ICapPublisher capPublisher)
        {
            _userContext = userContext;
            _logger = logger;
            _mapper = mapper;
            _capPublisher = capPublisher;
        }

        private async Task RaiseUserProfileChangedEvent(AppUser user)
        {
            if (_userContext.Entry(user).Property(u => u.Name).IsModified
                || _userContext.Entry(user).Property(u => u.Title).IsModified
                || _userContext.Entry(user).Property(u => u.Company).IsModified
                || _userContext.Entry(user).Property(u => u.Phone).IsModified
                || _userContext.Entry(user).Property(u => u.Avatar).IsModified)
            {
                await _capPublisher.PublishAsync("userapi.user.profile.changed", _mapper.Map<UserIdentity>(user));
            }
        }

        // GET: api/Users
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<AppUser>> Get()
        {
            var user = await _userContext.Users
                .AsNoTracking()
                .Include(u => u.Properties)
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            if (user == null)
            {
                throw new UserOperationException($"错误的用户上下文id={UserIdentity.UserId}");
            }
            return user;
        }

        [Route("")]
        [HttpPatch]
        public async Task<ActionResult<AppUser>> Patch([FromBody] JsonPatchDocument<AppUser> patch)
        {
            var user = await _userContext.Users
                .Include(u => u.Properties)
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            patch.ApplyTo(user);

            // foreach (var property in user.Properties)
            // {
            //     _context.Entry(property).State = EntityState.Detached;
            // }
            // var originProperties = await _context.UserProperties.AsNoTracking().Where(u => u.AppUserId == UserIdentity.UserId).ToListAsync();
            // var allProperties = originProperties.Union(user.Properties).Distinct();

            // var removedPropertites = originProperties.Except(user.Properties);
            // var newProperties = allProperties.Except(originProperties);

            // removedPropertites.Foreach(prop => _context.UserProperties.Remove(prop));
            // newProperties.Foreach(prop => _context.UserProperties.Add(prop));
            using (var trans = _userContext.Database.BeginTransaction(_capPublisher, autoCommit: true))
            {
                await RaiseUserProfileChangedEvent(user);
                _userContext.Users.Update(user);
                await _userContext.SaveChangesAsync();
                // _capBus.Publish("xxx.services.show.time", DateTime.Now);
            }

            return user;
        }


        [Route("check-or-create")]
        [HttpPost]
        public async Task<ActionResult<AppUser>> CheckOrCreate([FromBody] string phone)
        {
            //to do: check phone No formart

            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Phone == phone);
            if (user == null)
            {
                user = new AppUser { Phone = phone };
                _userContext.Users.Add(user);
                await _userContext.SaveChangesAsync();
            }
            return Ok(_mapper.Map<UserIdentity>(user));
        }

        [HttpGet]
        [Route("tags")]
        public async Task<ActionResult<List<UserTag>>> GetUserTags()
        {
            return await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync();
        }


        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<AppUser>> Search(string phone)
        {
            return await _userContext.Users.SingleOrDefaultAsync(u => u.Phone == phone);
        }

        [HttpPut]
        [Route("update-tags")]
        public async Task<ActionResult> UpdateUserTags([FromBody] List<string> tags)
        {
            var originTags = await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync();
            var newTags = tags.Except(originTags.Select(t => t.Tag));
            await _userContext.UserTags.AddRangeAsync(newTags.Select(t =>
                 new UserTag
                 {
                     UserId = UserIdentity.UserId,
                     Tag = t,
                     CreatedTime = DateTime.Now
                 }
            ));
            await _userContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("baseinfo/{userid}")]
        public async Task<ActionResult<UserIdentity>> BaseInfo(int userid)
        {
            var user = await _userContext.Users.SingleOrDefaultAsync(x => x.Id == userid);
            if (user == null) return NotFound();
            var userIdentity = _mapper.Map<UserIdentity>(user);

            return Ok(userIdentity);
        }
    }
}
