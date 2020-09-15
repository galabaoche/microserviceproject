using System;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using User.Identity.Services;
using IdentityServer4.Models;
using User.Identity.Dtos;
using System.Collections.Generic;
using System.Security.Claims;

namespace User.Identity.Authentication
{
    public class SmsAuthCodeValidator : IExtensionGrantValidator
    {
        private readonly IAuthCodeService _authCodeService;
        private readonly IUserService _userService;

        public SmsAuthCodeValidator(IAuthCodeService authCodeService, IUserService userService)
        {
            _authCodeService = authCodeService;
            _userService = userService;
        }
        public string GrantType => "sms_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            string phone = context.Request.Raw["phone"];
            string code = context.Request.Raw["auth_code"];
            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrEmpty(code))
            {
                context.Result = errorValidationResult;
                return;
            }

            if (!_authCodeService.Validate(phone, code))
            {
                context.Result = errorValidationResult;
                return;
            }

            UserIdentity userIdentity = await _userService.CheckOrCreate(phone);
            if (userIdentity == null)
            {
                context.Result = errorValidationResult;
                return;
            }
            var claims = new List<Claim>()
            {
                new Claim("name",userIdentity.Name??string.Empty),
                new Claim("company",userIdentity.Company??string.Empty),
                new Claim("title",userIdentity.Title??string.Empty),
                new Claim("avatar",userIdentity.Avatar??string.Empty),
                new Claim("phone",userIdentity.Phone??string.Empty),
            };
            context.Result = new GrantValidationResult(userIdentity.UserId.ToString(), GrantType, claims);
        }
    }
}
