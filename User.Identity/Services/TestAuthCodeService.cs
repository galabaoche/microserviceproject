using System;

namespace User.Identity.Services
{
    public class TestAuthCodeService : IAuthCodeService
    {
        public bool Validate(string phone, string authCode)
        {
            return true;
        }
    }
}
