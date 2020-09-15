using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recommend.API.Data;
using Recommend.API.Models;

namespace Recommend.API.Controllers
{
    [ApiController]
    [Route("api/recommends")]
    public class RecommendController : BaseController
    {
        private readonly ILogger<RecommendController> _logger;
        private readonly RecommendDbContext _recommendDbContext;

        public RecommendController(ILogger<RecommendController> logger, RecommendDbContext recommendDbContext)
        {
            _logger = logger;
            _recommendDbContext = recommendDbContext;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IList<ProjectRecommend>>> Get()
        {
            return await _recommendDbContext.ProjectRecommends.Where(r => r.UserId == UserIdentity.UserId)
                .ToListAsync();
        }
    }
}
