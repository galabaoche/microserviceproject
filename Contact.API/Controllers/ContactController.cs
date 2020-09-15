using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Contact.API.Models;
using Contact.API.Data;
using Contact.API.Services;
using System.Threading;
using Contact.API.ViewModels;
using DotNetCore.CAP;
using Contact.API.IntegrationEvents.Events;
using Contact.API.Dtos;

namespace Contact.API.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : BaseController
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IUserService _userService;
        private readonly IContactApplyRequestRepository _contactApplyRequestRepository;
        private readonly IContactRepository _contactRepository;

        public ContactController(ILogger<ContactController> logger, IContactApplyRequestRepository contactApplyRequestRepository, IUserService userService, IContactRepository contactRepository)
        {
            _logger = logger;
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
            _contactRepository = contactRepository;
        }

        /// <summary>
        /// 获取当前用户联系人列表
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Models.Contact>>> Get(CancellationToken cancellationToken)
        {
            return await _contactRepository.GetContactAsync(UserIdentity.UserId, cancellationToken);
        }

        /// <summary>
        /// 获取用户Id联系人列表
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<List<Models.Contact>>> Get(int userid, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetContactAsync(userid, cancellationToken);
        }

        /// <summary>
        /// 给好友打标签
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("tag")]
        public async Task<ActionResult> TagContact([FromBody] TagContactInputViewModel viewModel, CancellationToken cancellationToken)
        {
            var result = await _contactRepository.TagContactAsync(UserIdentity.UserId, viewModel.ContactId, viewModel.Tags, cancellationToken);
            if (!result)
            {
                // todo: log
                return BadRequest();
            }
            return Ok();
        }
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-requests")]
        public async Task<ActionResult<List<ContactApplyRequest>>> GetApplyRequests(CancellationToken cancellationToken)
        {
            var result = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId, cancellationToken);
            return result;
        }

        /// <summary>
        /// 添加好友请求
        /// </summary
        /// <param name="userId">要添加的好友Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-request/{userId}")]
        public async Task<ActionResult> AddApplyRequest([FromRoute] int userId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserIdentityAsync(userId);
            if (user == null)
            {
                throw new Exception("用户参数错误");
            }
            var result = await _contactApplyRequestRepository.AddRequestAsync(new ContactApplyRequest
            {
                UserId = userId,
                Name = UserIdentity.Name,
                ApplierId = UserIdentity.UserId,
                ApplyTime = DateTime.Now,
                Avatar = UserIdentity.Avatar,
                Title = UserIdentity.Title,
                Company = UserIdentity.Company
            }, cancellationToken);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// 同意好友请求
        /// </summary>
        /// <param name="applierId">申请人Id</param>
        /// <returns></returns>
        [HttpPut]
        [Route("approval-request")]
        public async Task<ActionResult> ApprovalApplyRequest([FromBody] int applierId, CancellationToken cancellationToken)
        {
            var result = await _contactApplyRequestRepository.ApprovalAsync(UserIdentity.UserId, applierId, cancellationToken);
            if (!result)
            {
                return BadRequest();
            }

            var applier = await _userService.GetUserIdentityAsync(applierId);
            var userInfo = await _userService.GetUserIdentityAsync(UserIdentity.UserId);

            await _contactRepository.AddContactAsync(UserIdentity.UserId, applier, cancellationToken);
            await _contactRepository.AddContactAsync(applierId, userInfo, cancellationToken);
            return Ok();
        }

        [NonAction]
        [CapSubscribe("userapi.user.profile.changed")]
        public async Task ConsumerUserInfoChangedEvent(UserInfoChangedEvent @event)
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
