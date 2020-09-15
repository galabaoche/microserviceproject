using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Project.API.Dtos;

namespace Project.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public UserIdentity UserIdentity
        {
            get
            {
                var user = new UserIdentity();

                user.Avatar = User.Claims.First(x => x.Type == "avatar").Value;
                user.Company = User.Claims.First(x => x.Type == "company").Value;
                user.Name = User.Claims.First(x => x.Type == "name").Value;
                user.Phone = User.Claims.First(x => x.Type == "phone").Value;
                user.Title = User.Claims.First(x => x.Type == "title").Value;
                user.UserId = int.Parse(User.Claims.First(x => x.Type == "sub").Value);

                return user;
            }
        }
    }
}