using System.Linq;
using Contact.API.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace Contact.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public UserIdentity UserIdentity => new UserIdentity
        {
            Avatar = User.Claims.First(x => x.Type == "avatar").Value,
            Company = User.Claims.First(x => x.Type == "company").Value,
            Name = User.Claims.First(x => x.Type == "name").Value,
            Phone = User.Claims.First(x => x.Type == "phone").Value,
            Title = User.Claims.First(x => x.Type == "title").Value,
            UserId = int.Parse(User.Claims.First(x => x.Type == "sub").Value)
        };
    }
}