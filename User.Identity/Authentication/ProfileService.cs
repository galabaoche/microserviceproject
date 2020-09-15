using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace User.Identity.Authentication
{
    public class ProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (context.Subject.Claims.Any())
            {
                context.IssuedClaims = context.Subject.Claims.ToList();
            }
            await Task.CompletedTask;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            context.IsActive = true;
            if (string.IsNullOrWhiteSpace(subject.GetSubjectId()))
            {
                context.IsActive = false;
            }

            await Task.CompletedTask;
        }
    }
}
